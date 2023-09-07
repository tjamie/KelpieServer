using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KelpieServer;
using KelpieServer.Models;
using KelpieServer.Mappers;

namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return BadRequest();
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDto projectDto)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'EF_DataContext.Projects'  is null.");
            }

            var projectMapper = new ProjectMapper();
            var project = projectMapper.MapToEntity(projectDto);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
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
    }
}
