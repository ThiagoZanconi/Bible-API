using Microsoft.AspNetCore.Mvc;
using MiProyectoBackend.database;

[ApiController]
[Route("api/[controller]")]
public class HelloController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        return Ok("Hello!");
    }
}