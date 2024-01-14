using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KelpieServer;
using KelpieServer.Models;
using KelpieServer.Mappers;
using System.Security.Claims;

namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly EF_DataContext _context;

        public ProjectsController(EF_DataContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var projectsList = await _context.Projects.ToListAsync();
            List<ProjectResponseDto> projectResponseList = new List<ProjectResponseDto>();
            foreach (Project project in projectsList)
            {
                projectResponseList.Add(new ProjectResponseDto
                {
                    Name = project.Name
                });
            }
            return Ok(projectResponseList);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(string id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectResponse = new ProjectResponseDto
            {
                Name = project.Name
            };

            return Ok(projectResponse);
        }

        // GET: api/Projects/5/Datapoints
        [HttpGet("{id}/datapoints")]
        public IActionResult GetProjectDatapoints(string id)
        {
            //TODO add authentication such that only users assigned to project can view datapoints

            if (_context.Projects == null)
            {
                return NotFound();
            }
            
            var project = _context.Projects
                .Include (p => p.Datapoints)
                .SingleOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            var datapointsResponse = new List<DatapointDto>();
            var datapointMapper = new DatapointMapper();
            foreach (Datapoint datapoint in project.Datapoints)
            {
                datapointsResponse.Add(datapointMapper.MapToEntity(datapoint));
            }

            return Ok(datapointsResponse);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(string id, ProjectDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest("Project ID mismatch");
            }

            // Reject if user not assigned to project
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Invalid claims identity");
            }
            var _identity = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!UserProjectExists(int.Parse(_identity), projectDto.Id))
            {
                return BadRequest("Target project has not been assigned to user");
            }

            try
            {
                var targetProject = await _context.Projects.FindAsync(id);

                if (targetProject == null)
                {
                    return NotFound();
                }

                var projectMapper = new ProjectMapper();
                projectMapper.MapToEntity(projectDto, ref targetProject);
                await _context.SaveChangesAsync();

                return Ok(new ProjectResponseDto
                {
                    Name = projectDto.Name,
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // PUT: api/Projects/5/sync
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // This is very very very ugly right now. Wanted to avoid making several API calls from a single button within the app.
        [HttpPut("{id}/sync")]
        public async Task<IActionResult> SyncProject(string id, ProjectSyncDto projectSyncDto)
        {
            var projectDto = projectSyncDto.ProjectDto;
            var projectDatapointsInput = projectSyncDto.DatapointDtoList;
            DatapointMapper datapointMapper = new DatapointMapper();

            if (projectDto == null)
            {
                return BadRequest("No project submitted");
            }

            if (id != projectDto.Id)
            {
                return BadRequest("Project ID mismatch");
            }

            // Get project, then create it if it doesn't exist in DB
            var targetProject = _context.Projects
                .Include(p => p.Datapoints)
                .SingleOrDefault(p => p.Id == id);

            if (targetProject == null)
            {
                //return NotFound("Project does not exist");
                // post project if it does not already exist in DB. This should also associated it with the active user.
                await PostProject(projectDto);

                // Attempt to get project again. If for some reason it still doesn't exist, reject.
                targetProject = _context.Projects
                    .Include(p => p.Datapoints)
                    .SingleOrDefault(p => p.Id == id);

                if (targetProject == null)
                {
                    return NotFound("Project does not exist");
                }
            }

            // Reject if user not assigned to project
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return BadRequest("Invalid claims identity");
            }
            var _identity = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!UserProjectExists(int.Parse(_identity), projectDto.Id))
            {
                return BadRequest("Target project has not been assigned to user");
            }

            try
            {
                //var targetProject = await _context.Projects.FindAsync(id);
                //var targetProject = _context.Projects
                //    .Include(p => p.Datapoints)
                //    .SingleOrDefault(p => p.Id == id);

                //if (targetProject == null)
                //{
                //    return NotFound();
                //}

                #region update database and prepare response object
                // Only apply update to database if targetProject date is older than projectDto

                ProjectSyncDto responseDto = new ProjectSyncDto();
                if (targetProject.Date > projectDto.Date)
                {
                    // targetProject is newer -- add database data to response
                    ProjectMapper projectMapper = new ProjectMapper();
                    responseDto.ProjectDto = projectMapper.MapToEntity(targetProject);

                }
                else
                {
                    // targetProject is either newer or same age as projectDto. Update project, then proceed to associated datapoints.
                    var projectMapper = new ProjectMapper();
                    projectMapper.MapToEntity(projectDto, ref targetProject);
                    await _context.SaveChangesAsync();
                }

                // Handle datapoints -- for each that's newer in database than in submitted data, add updated version to response object
                // else update database with newer version submitted by user

                // iterate through each relevant datapoint in DB, update DB or add to response accordingly, then remove from projectDatapoints input
                // For each datapoint that is in database but not in projectDatapoints input, automatically add to response
                foreach (Datapoint datapoint in targetProject.Datapoints)
                {
                    DatapointDto? pairedDatapointDto = projectDatapointsInput?.SingleOrDefault(d => d.Id == datapoint.Id);

                    // if null (ie, datapoint in DB but not submitted by user), add to response
                    if (pairedDatapointDto == null)
                    {
                        responseDto.AddDatapointDto(datapointMapper.MapToEntity(datapoint));
                    }
                    // if not null, add to response if version in DB is newer, else update DB version with user submission
                    else if (datapoint.Date > pairedDatapointDto.Date)
                    {
                        responseDto.AddDatapointDto(datapointMapper.MapToEntity(datapoint));
                    }
                    // if not null and user's submitted version is newer, update DB version and do not add to response object
                    else
                    {
                        var contextDp = await _context.Datapoints.FindAsync(datapoint.Id);
                        if (contextDp != null)
                        {
                            datapointMapper.MapToEntity(pairedDatapointDto, ref contextDp);
                            await _context.SaveChangesAsync();
                        }
                    }
                    // remove from list of input datapoints
                    projectDatapointsInput?.RemoveAll(d => d.Id == datapoint.Id);
                }

                // Any remaining items in projectDatapointsInput are not currently in the DB, so update the DB accordingly
                if (projectDatapointsInput != null && projectDatapointsInput.Count > 0)
                {
                    foreach (DatapointDto datapointDto in projectDatapointsInput)
                    {
                        _context.Datapoints.Add(datapointMapper.MapToEntity(datapointDto));
                    }
                    await _context.SaveChangesAsync();
                }

                return Ok(responseDto);
                #endregion
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDto projectDto)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'EF_DataContext.Projects'  is null.");
            }

            // Add project
            var projectMapper = new ProjectMapper();
            var project = projectMapper.MapToEntity(projectDto);
            _context.Projects.Add(project);

            // Associate new project with active user
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            int httpUserId = -1;
            if (identity != null)
            {
                var _identity = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var _role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                httpUserId = int.Parse(_identity);
            }
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(httpUserId);
            if (user == null)
            {
                return NotFound();
            }
            _context.UserProject.Add(new UserProject
            {
                UserId = user.Id,
                ProjectId = project.Id
            });

            // Save changes
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, projectDto);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(string id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool UserProjectExists(int userId, string projectId)
        {
            var user = _context.Users
                .Include(u => u.UserProjects)
                .ThenInclude(up => up.Project)
                .SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            var projects = user.UserProjects.Select(up => up.Project).ToList();
            return (projects?.Any(e => e.Id == projectId)).GetValueOrDefault();
        }
    }
}
