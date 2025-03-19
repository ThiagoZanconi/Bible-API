using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.model;

[ApiController]
[Route("api/[controller]")]
public class RegisterController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IResult> Register([FromBody] RegisterCredentials userRegister)
    {
        User? user = await _context.users.FirstOrDefaultAsync(u => u.Email == userRegister.Email);
        if(user!=null){
            return Results.InternalServerError("Email already used - ");
        }

        user = new User(userRegister.Name,userRegister.Email, userRegister.Password,"User");
        
        try{
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }catch(Exception e){
            return Results.InternalServerError("Error: Credenciales invalidas - "+e.Message);
        }
        
        return Results.Created();
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

        var user = await _context.users.FindAsync(user_id);

        if (user == null){
            return Results.NotFound($"No se encontró el usuario con ID {user_id}.");
        }

        _context.users.Remove(user);
        await _context.SaveChangesAsync();

        return Results.Ok(new { message = "¡Usuario eliminado exitosamente!" });
    }
}