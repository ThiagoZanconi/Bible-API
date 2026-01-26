using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.postgres_model;

[ApiController]
[Route("api/[controller]")]
public class RegisterController(PostgresContext context) : ControllerBase
{
    private readonly PostgresContext _context = context;

    [HttpPost]
    public async Task<IResult> Register(
        [FromBody] RegisterCredentials userRegister,
        [FromServices] IPasswordHasher<User> passwordHasher)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userRegister.Email);

        if (existingUser != null)
            return Results.Conflict("Email already used");

        var user = new User
        {
            Name = userRegister.Name,
            Email = userRegister.Email,
            Role = "User"
        };

        user.PasswordHash = passwordHasher.HashPassword(user, userRegister.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Results.Created($"/users/{user.Id}", null);
    }

    [HttpDelete]
    [Authorize(Roles = "User")]
    public async Task<IResult> DeleteAccount()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int user_id = int.Parse(userIdClaim);

        var user = await _context.Users.FindAsync(user_id);

        if (user == null){
            return Results.NotFound($"No se encontró el usuario con ID {user_id}.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Results.Ok(new { message = "¡Usuario eliminado exitosamente!" });
    }

}