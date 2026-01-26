using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.postgres_model;

//[ApiController]
//[Route("api/[controller]")]
public class PostgresSeeder(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost("seed-translations")]
    public async Task<IResult> SeedTranslations([FromServices] PostgresContext pgContext)
    {
        var mysqlTranslations = await _context.translations.ToListAsync();

        var pgTranslations = mysqlTranslations.Select(b => new MiProyectoBackend.postgres_model.Translation{
            Id = b.identifier, 
            Name = b.name, 
            Language = b.language, 
            LanguageCode = b.languageCode, 
            License = b.license
            }).ToList();

        await pgContext.Translations.AddRangeAsync(pgTranslations);
        await pgContext.SaveChangesAsync();

        return Results.Ok(new { message = "Translations seeded to PostgreSQL", count = pgTranslations.Count });
    }

    [HttpPost("seed-books")]
    public async Task<IResult> SeedBooks([FromServices] PostgresContext pgContext)
    {
        var mysqlBooks = await _context.books.ToListAsync();

        var pgKJVBooks = mysqlBooks.Select(b => new MiProyectoBackend.postgres_model.Book{
            Id = b.id, 
            Name = b.name, 
            TranslationId = "kjv"
        }).ToList();

        await pgContext.Books.AddRangeAsync(pgKJVBooks);
        await pgContext.SaveChangesAsync();

        return Results.Ok(new { message = "Books seeded to PostgreSQL", count = pgKJVBooks.Count });
    }
    
    [HttpPost("seed-chapters")]
    public async Task<IResult> SeedChapters([FromServices] PostgresContext pgContext)
    {
        var mysqlChapters = await _context.chapters.ToListAsync();

        var pgBESChapters = mysqlChapters.Select(b => new MiProyectoBackend.postgres_model.Chapter{
            BookId = b.book_id, 
            Chp = b.chapter, 
            TranslationId = "BES"
        }).ToList();

        await pgContext.Chapters.AddRangeAsync(pgBESChapters);
        await pgContext.SaveChangesAsync();

        return Results.Ok(new { message = "Books seeded to PostgreSQL", count = pgBESChapters.Count });
    }

    [HttpPost("seed-verses")]
    public async Task<IResult> SeedVerses([FromServices] PostgresContext pgContext)
    {
        var mysqlVerses = await _context.verses.ToListAsync();

        var pgVerses = mysqlVerses.Select(b => new MiProyectoBackend.postgres_model.Verse{
            BookId = b.book_id, 
            Chapter = b.chapter, 
            Vrs = b.verse,
            Text = b.text,
            TranslationId = b.translation_id
        }).ToList();

        await pgContext.Verses.AddRangeAsync(pgVerses);
        await pgContext.SaveChangesAsync();

        return Results.Ok(new { message = "Books seeded to PostgreSQL", count = pgVerses.Count });
    }
}