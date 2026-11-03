
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Lab3API.Data;
using Lab3API.Models;

namespace Lab3API.Controllers;

public class AuthRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = "";
}

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] AuthRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        if (await _context.Users
            .AnyAsync(u => u.Email == email))
        {
            return Conflict(new {
                message = "Email đã tồn tại"
            });
        }

        var user = new User
        {
            Email = email
        };

        var hasher = new PasswordHasher<User>();

        user.PasswordHash = hasher.HashPassword(
            user, request.Password);

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new {
                message = "Email đã tồn tại"
            });
        }

        return Ok(new {
            message = "Đăng ký thành công"
        });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] AuthRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return Unauthorized(new {
                message = "Email hoặc mật khẩu không đúng"
            });
        }

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new {
                message = "Email hoặc mật khẩu không đúng"
            });
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()
                ),
                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email
                )
            },
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return Ok(new
        {
            message = "Đăng nhập thành công",
            email = user.Email,
            token = new JwtSecurityTokenHandler()
                .WriteToken(token)
        });
    }
}
