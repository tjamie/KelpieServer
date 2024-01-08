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
        [HttpPut("{id}/sync")]
        public async Task<IActionResult> SyncProject(string id, ProjectDto projectDto)
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

                #region prepare response object
                // Only apply update if targetProject date is older than projectDto

                ProjectSyncResponseDto responseDto = new ProjectSyncResponseDto();
                if (targetProject.Date > projectDto.Date)
                {
                    // targetProject is newer -- add database data to response
                    ProjectMapper projectMapper = new ProjectMapper();
                    responseDto.ProjectDto = projectMapper.MapToEntity(targetProject);

                    // Also (eventually) proceed to datapoints associated with target project id
                }
                else
                {
                    // targetProject is either newer or same age as projectDto. Update project, then proceed to associated datapoints.
                    var projectMapper = new ProjectMapper();
                    projectMapper.MapToEntity(projectDto, ref targetProject);
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
