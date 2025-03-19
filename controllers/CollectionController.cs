using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.model;

[ApiController]
[Route("api/[controller]")]
public class CollectionController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IResult> GetCollections(){

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int userId = int.Parse(userIdClaim);

        List<Collection> collections = await _context.collections.Where(c => c.user_id == userId).ToListAsync();

        return Results.Ok(collections);
    }

    [HttpGet("{name}")]
    [Authorize(Roles = "User")]
    public async Task<IResult> GetVerseCollection(string name){

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int userId = int.Parse(userIdClaim);

        var collection = await _context.collections.FirstOrDefaultAsync(c => c.name == name);

        if(collection == null){
            return Results.NotFound(new { message = "Error 404: Coleccion no encontrada" });
        }

        var verse_collections = await _context.verse_collection.Where(c => c.collection_id == collection.id).ToListAsync();

        return Results.Ok(verse_collections);
    }

    [HttpPost("{name}")]
    [Authorize(Roles = "User")]
    public async Task<IResult> CreateCollection(string name){
        // Obtener el ID del usuario autenticado
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int user_id = int.Parse(userIdClaim);

        // Crear la nueva colección
        var collection = new Collection(user_id, name, DateTime.UtcNow);

        // Guardar en la base de datos
        _context.collections.Add(collection);
        await _context.SaveChangesAsync();

        return Results.Created($"/collections/{collection.id}", collection);
    }

    [HttpPost("{name}/{book_id}/{chapter}:{verse}")]
    [Authorize(Roles = "User")]
    public async Task<IResult> AddVerseToCollection(string name, string book_id, int chapter, int verse){
        // Obtener el ID del usuario autenticado
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int user_id = int.Parse(userIdClaim);

        // Crear la nueva colección
        var collection = await _context.collections.FirstOrDefaultAsync(c => c.name == name);

        try{
            if(collection==null){
                collection = new Collection(user_id, name, DateTime.UtcNow);

                // Guardar en la base de datos
                _context.collections.Add(collection);
            }
            Verse_Collection verse_Collection = new Verse_Collection(collection.id, book_id, chapter, verse);
            _context.verse_collection.Add(verse_Collection);

            await _context.SaveChangesAsync();
            return Results.Created();

        }catch(Exception e){
            return Results.InternalServerError("Error: Parametros invalidos - "+e.Message);
        }
        
    }

    [HttpDelete("{name}")]
    [Authorize(Roles = "User")]
    public async Task<IResult> DeleteCollection(string name){
        // Obtener el ID del usuario autenticado
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int user_id = int.Parse(userIdClaim);

        // Crear la nueva colección
        var collection =await _context.collections.FirstOrDefaultAsync(c => c.name == name);

        // Guardar en la base de datos
        if(collection!=null){
            _context.collections.Remove(collection);
            await _context.SaveChangesAsync();
        }

        return Results.NoContent();
    }
}