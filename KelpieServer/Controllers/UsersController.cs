using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KelpieServer;
using KelpieServer.Models;
using System.Text;
using KelpieServer.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        //private readonly EF_DataContext _context;
        private readonly IDataContext _context;

        public UsersController(IDataContext context)
        {
            _context = context;
        }

        private static string HashString(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(sourceBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            List<UserResponseDto> responseList = new List<UserResponseDto>();
            var userList = await _context.Users.ToListAsync();

            foreach (var user in userList)
            {
                responseList.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username
                });
            }

            return Ok(responseList);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var userResponse = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username
            };
            
            return Ok(userResponse);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, UserDto userDto)
        {
            // Allow if target user = token user OR if token user is admin
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            int httpUserId = -1;
            bool httpUserAdmin = false;
            if (identity != null)
            {
                var _identity = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var _role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                httpUserId = int.Parse(_identity);
                httpUserAdmin = _role != null && _role.Value == "Admin" ? true : false;
            }

            if (id != userDto.Id || (id != httpUserId && !httpUserAdmin))
            {
                return BadRequest();
            }

            if (userDto.Password != null)
            {
                userDto.Password = HashString(userDto.Password);
            }

            try
            {
                var targetUser = await _context.Users.FindAsync(id);

                if (targetUser == null)
                {
                    return NotFound();
                }

                var userMapper = new UserMapper();
                userMapper.MapToEntity(userDto, ref targetUser);
                //_context.Entry(targetUser).State = EntityState.Modified;
                _context.MarkAsModified(targetUser);
                await _context.SaveChangesAsync();

                return Ok(new UserResponseDto
                {
                    Id = userDto.Id,
                    Username = userDto.Username
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            // TODO check if username already exists
            if (_context.Users == null)
            {
                return Problem("Entity set 'EF_DataContext.Users'  is null.");
            }
            if (_context.Users.Any(u => u.Username.ToLower() == userDto.Username.ToLower())){
                return BadRequest("Username already exists.");
            }
            userDto.Password = HashString(userDto.Password);
            var userMapper = new UserMapper();
            var user = userMapper.MapToEntity(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Allow if target user = token user OR if token user is admin
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            int httpUserId = -1;
            bool httpUserAdmin = false;
            if (identity != null)
            {
                var _identity = identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var _role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                httpUserId = int.Parse(_identity);
                httpUserAdmin = _role != null && _role.Value == "Admin" ? true : false;
            }

            if (id != httpUserId && !httpUserAdmin)
            {
                return BadRequest();
            }

            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // User project assignment
        // get all projects assigned to user
        // GET: api/Users/5/Projects
        [HttpGet("{id}/projects")]
        public IActionResult GetUserProjects(int id)
        {
            if (_context.UserProject == null)
            {
                return NotFound();
            }
            if (!UserExists(id))
            {
                return NotFound();
            }

            //var user = await _context.Users.FindAsync(id);
            var user = _context.Users
                .Include(u => u.UserProjects)
                .ThenInclude(up => up.Project)
                .SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var projects = user.UserProjects.Select(up => up.Project).ToList();
            List<ProjectResponseDto> responseList = new List<ProjectResponseDto>();
            foreach (var project in projects)
            {
                responseList.Add(new ProjectResponseDto
                {
                    Name = project.Name
                });
            }
            return Ok(responseList);
        }

        // assign project to user
        // POST: api/Users/5/Projects
        [HttpPost("{userId}/projects")]
        public async Task<IActionResult> AssignProject(int userId, string projectId)
        {
            if (_context.Users == null || _context.Projects == null)
            {
                return NotFound();
            }
            if (!UserExists(userId) || !ProjectExists(projectId))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user == null || project == null)
            {
                return NotFound();
            }

            _context.UserProject.Add(new UserProject
            {
                UserId = userId,
                ProjectId = projectId,
                User = user,
                Project = project
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ie, unassign a project
        // DELETE: api/Users/5/Projects
        [HttpDelete("{userId}/projects")]
        public async Task<IActionResult> UnassignProject(int userId, string projectId)
        {
            if (_context.Users == null || _context.Projects == null)
            {
                return NotFound();
            }
            if (!UserExists(userId) || !ProjectExists(projectId))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            if (user == null || project == null)
            {
                return NotFound();
            }

            if (!UserProjectExists(userId, projectId))
            {
                return NotFound();
            }

            user.Projects.Remove(project);
            project.Users.Remove(user);

            //var userProjectTarget = await _context.UserProjects
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
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
