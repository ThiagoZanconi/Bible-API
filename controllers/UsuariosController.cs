using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.model;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.users.ToListAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> CrearUsuario([FromBody] User usuario)
    {
        // Aquí puedes agregar validaciones si lo deseas

        // Se agrega el usuario a la base de datos
        _context.users.Add(usuario);
        await _context.SaveChangesAsync();

        // Retorna el usuario creado con el Id asignado por la base de datos
        return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
    }
}