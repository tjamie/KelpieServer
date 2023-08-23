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

namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EF_DataContext _context;

        public UsersController(EF_DataContext context)
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
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            if (user.Password != null)
            {
                user.Password = HashString(user.Password);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'EF_DataContext.Users'  is null.");
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
        //[HttpGet("{id}/projects")]
        //public IActionResult GetUserProjects(int id)
        //{
        //    if (_context.UserProject == null)
        //    {
        //        return NotFound();
        //    }
        //    //var user = await _context.Users.FindAsync(id);
        //    var user = _context.Users
        //        .Include(u => u.UserProjects)
        //        .ThenInclude(up => up.Project)
        //        .SingleOrDefault(u => u.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var projects = user.UserProjects.Select(up => up.Project).ToList();
        //    return Ok(projects);
        //    //return await _context.Users.ToListAsync();
        //}
        // assign project to user
        // POST: api/Users/5/Projects
        [HttpPost("{userId}/projects")]
        public async Task<IActionResult> AssignProject(int userId, string projectId)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            if (!UserExists(userId))
            {
                return NotFound();
            }
            if (!ProjectExists(projectId))
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);
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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool ProjectExists(string id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
