using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.postgres_model;

[ApiController]
[Route("api/[controller]")]
public class CollectionController(PostgresContext context) : ControllerBase
{
    private readonly PostgresContext _context = context;

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IResult> GetCollections(){

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int userId = int.Parse(userIdClaim);

        List<Collection> collections = await _context.Collections.Where(c => c.UserId == userId).ToListAsync();

        return Results.Ok(collections);
    }

    [HttpGet("{name}/{translation_id}")]
    [Authorize(Roles = "User")]
    public async Task<IResult> GetVerseCollection(string name, string translation_id){

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            return Results.Unauthorized();
        }

        int userId = int.Parse(userIdClaim);

        var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Name == name && c.UserId == userId);

        if(collection == null){
            return Results.NotFound(new { message = "Error 404: Coleccion no encontrada" });
        }

        var verse_collections = await _context.VerseCollection.Where(v => v.CollectionId == collection.Id).ToListAsync();
        var verses = new List<Verse>();
        Console.WriteLine($"Verse collections count: {verse_collections.Count}");
        foreach(var v in verse_collections){
            
            Verse? verse = await _context.Verses.FirstOrDefaultAsync(verse => verse.BookId == v.BookId 
            && verse.Chapter == v.Chapter && verse.Vrs == v.Verse && verse.TranslationId == translation_id);
            if(verse!=null){
                verses.Add(verse);
            }
        }
        
        return Results.Ok(verses);
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
        var collection = new Collection{UserId = user_id, Name = name};

        // Guardar en la base de datos
        _context.Collections.Add(collection);
        await _context.SaveChangesAsync();

        return Results.Created($"/collections/{collection.Name}", collection);
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
        var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Name == name);

        try{
            if(collection==null){
                collection = new Collection{UserId = user_id, Name = name};

                // Guardar en la base de datos
                _context.Collections.Add(collection);
            }
            var verse_Collection = new VerseCollection{BookId = book_id, Chapter = chapter, Verse = verse, CollectionId = collection.Id};
            _context.VerseCollection.Add(verse_Collection);

            await _context.SaveChangesAsync();
            return Results.Created();

        }catch(Exception e){
            return Results.Problem(
                title: "Internal Server Error",
                detail: "Error: Parametros invalidos - "+e.Message,
                statusCode: 500
            );
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
        var collection =await _context.Collections.FirstOrDefaultAsync(c => c.Name == name);

        // Guardar en la base de datos
        if(collection!=null){
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
        }

        return Results.NoContent();
    }
}