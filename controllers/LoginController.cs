using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiProyectoBackend.postgres_model;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IConfiguration config, PostgresContext context) : ControllerBase
{

    private readonly IConfiguration _config = config;
    private readonly PostgresContext _context = context;


    [HttpGet]
    [Authorize(Roles = "User")]
    public IResult ValidToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok("Token Valido");
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody] LoginCredentials userLogin,
        [FromServices] IPasswordHasher<User> passwordHasher)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userLogin.Email);

        if (user == null)
            return Unauthorized("Credenciales incorrectas");

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            userLogin.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Credenciales incorrectas");

        var token = Generate(user);

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok("Sesión cerrada");
    }

    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Crear los claims

        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

        //Crear el token
        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}