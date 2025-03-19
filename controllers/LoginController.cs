using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiProyectoBackend.database;
using MiProyectoBackend.model;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IConfiguration config, AppDbContext context) : ControllerBase
{

    private readonly IConfiguration _config = config;
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginCredentials userLogin)
    {
        var user = await Auth(userLogin);
        if (user != null)
        {
            var token = Generate(user); // Genera el JWT

            // Devuelve el token JWT directamente en la respuesta
            return Ok(new { token }); // Puedes devolverlo como un objeto JSON con la propiedad 'token'
        }

        return NotFound("Credenciales incorrectas");
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok("Sesión cerrada");
    }

    private async Task<User?> Auth(LoginCredentials userLogin)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.Email == userLogin.Email && u.Password == userLogin.Password);
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
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}