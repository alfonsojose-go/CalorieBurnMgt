using CalorieBurnMgt.Data;
using CalorieBurnMgt.DTOs;
using CalorieBurnMgt.Models;
using CalorieBurnMgt.Services;
using BCrypt.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalorieBurnMgt.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;

        public UsersController(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(RegisterRequest dto)
        //{
        //    if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        //        return BadRequest(new { message = "User already exists" });

        //    var user = new User
        //    {
        //        Username = dto.Uname,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "User registered successfully" });
        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginRequest dto)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(x =>
        //        x.Username == dto.Username);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        //        return BadRequest(new { message = "Invalid credentials" });

        //    var token = _jwt.GenerateToken(user);

        //    return Ok(new { token });
        //}

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedRoute()
        {
            var username = User.Claims.First(c => c.Type == "username").Value;
            return Ok(new { message = $"Hello user {username}, you have accessed a protected route!" });
        }
    }
}
