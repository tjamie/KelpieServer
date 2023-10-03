using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using KelpieServer.Models;

[assembly: InternalsVisibleTo("KelpieServer.Tests")]

namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDataContext _context;
        private readonly IConfiguration _configuration;
        //private readonly ILogger<AuthController> _logger;

        //public AuthController(IConfiguration configuration, ILogger<AuthController> logger, IDataContext context)
        public AuthController(IConfiguration configuration, IDataContext context)
        {
            _configuration = configuration;
            //_logger = logger;
            _context = context;
        }

        // api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = Authenticate(userLoginDto);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var token = GenerateToken(user);
            return Ok(token);
        }

        internal string GenerateToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (user.Admin)
            {
                claims = claims.Append(new Claim(ClaimTypes.Role, "Admin")).ToArray();
            }

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDto Authenticate(UserLoginDto userLoginDto)
        {
            string userPassword = HashString(userLoginDto.Password);
            var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == userLoginDto.Username.ToLower() && u.Password == userPassword);
            if (user == null)
            {
                return null;
            }
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Admin = user.Admin,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email ?? null
            };
            return userDto;
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
    }
}
