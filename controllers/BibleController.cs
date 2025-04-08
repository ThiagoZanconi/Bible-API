using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.model;

[ApiController]
[Route("api/[controller]")]
public class BibleController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    public static readonly string[] OrderedBooks =
    {
        "GEN", "EXO", "LEV", "NUM", "DEU", 
        "JOS", "JDG", "RUT", "1SA", "2SA",
        "1KI", "2KI", "1CH", "2CH", "EZR",
        "NEH", "EST", "JOB", "PSA", "PRO",
        "ECC", "SNG", "ISA", "JER", "LAM",
        "EZK", "DAN", "HOS", "JOL", "AMO",
        "OBA", "JON", "MIC", "NAM", "HAB",
        "ZEP", "HAG", "ZEC", "MAL",
        "MAT", "MRK", "LUK", "JHN", "ACT",
        "ROM", "1CO", "2CO", "GAL", "EPH",
        "PHP", "COL", "1TH", "2TH",
        "1TI", "2TI", "TIT", "PHM", "HEB",
        "JAS", "1PE", "2PE", "1JN", "2JN",
        "3JN", "JUD", "REV"
    };

    [HttpGet]
    public async Task<IResult> GetBible()
    {
        List<Book> books = new List<Book>();

        foreach(var b in OrderedBooks){
            var book = await _context.books.FirstOrDefaultAsync(bo => bo.id == b);
            if(book!=null){
                books.Add(book);
            }
        }

        Console.WriteLine(books.Count);
        if (books.Count!=66)
        {
            return Results.NotFound("Error: Couldnt retrive all books");
        }

        return Results.Ok(books);
    }

    [HttpGet("{book_id}")]
    public async Task<IResult> GetBook(string book_id)
    {
        var chapters = await _context.chapters
        .Where(c => c.book_id == book_id)
        .ToListAsync();

        if (chapters.Count==0)
        {
            return Results.NotFound("Error: Chapter not found");
        }
        return Results.Ok(chapters);
    }

    [HttpGet("{translation_id}/{book_id}/{query}")]
    public async Task<IResult> GetVersesFromQuery(string translation_id, string book_id, string query)
    {
        List<string> querySplit = query.Split(':').ToList();
        try{
            int chapter = int.Parse(querySplit[0]);
            if(querySplit.Count>1){
                List<string> querySplit2 = querySplit[1].Split('-').ToList();
                int verse1 = int.Parse(querySplit2[0]);
                if(querySplit2.Count>1){
                    int verse2 = int.Parse(querySplit2[1]);
                    return await GetContinuationOfVerses(translation_id, book_id, chapter, verse1, verse2);
                }
                else{
                    return await GetVerse(translation_id, book_id, chapter, verse1);
                }
            }
            else{
                return await GetChapter(translation_id, book_id, chapter);
            }
        }catch(FormatException e){
            return Results.InternalServerError(e.Message);
        }    
    }

    //[HttpGet("{book_id}/{chapter}")]
    private async Task<IResult> GetChapter(string translation_id, string book_id, int chapter)
    {
        var verses = await _context.verses
        .Where(v => v.translation_id == translation_id && v.book_id == book_id && v.chapter == chapter)
        .ToListAsync();

        if (verses.Count==0)
        {
            return Results.NotFound("Error: Chapter not found");
        }
        return Results.Ok(verses);
    }

    //[HttpGet("{book_id}/{chapter}:{verse}")]
    private async Task<IResult> GetVerse( string translation_id, string book_id, int chapter, int verse)
    {
        var v = await _context.verses.FirstOrDefaultAsync(v => v.translation_id == translation_id && v.book_id == book_id && v.chapter == chapter && v.verse == verse);

        if (v == null)
        {
            return Results.NotFound("Error: Verse not found");
        }
        return Results.Ok(v);
    }

    //[HttpGet("{book_id}/{chapter}:{verse1}-{verse2}")]
    private async Task<IResult> GetContinuationOfVerses(string translation_id, string book_id, int chapter, int verse1, int verse2)
    {
        if(verse1>=verse2){
            return Results.InternalServerError("Error: Verse 1 should preceed verse 2");
        }
        var v = await _context.verses.Where(v => v.translation_id == translation_id && v.book_id == book_id && v.chapter == chapter && v.verse >= verse1 && v.verse<=verse2).ToListAsync();

        if (v == null)
        {
            return Results.NotFound("Error: Verse not found");
        }
        return Results.Ok(v);
    }

    [HttpGet("{translation_id}/keywords/{keywords}")]
    public async Task<IResult> GetVersesFilteredByKeywords(string translation_id, string keywords)
    {
        var parsedKeyword = keywords.Replace('_',' ');
        List<string> keywordList = parsedKeyword.Split(',').ToList();
        if(keywordList.Count == 0){
            return Results.InternalServerError("Error: Bad input of keywords");
        }
        var verses = await _context.verses
        .Where(v => v.text.Contains(keywordList[0]) && v.translation_id == translation_id)
        .ToListAsync();

        for (int i = 1; i < keywordList.Count; i++)
        {
            verses = [.. verses.Where(v => v.text.IndexOf(keywordList[i], StringComparison.OrdinalIgnoreCase) >= 0)];
        }

        if (verses.Count == 0)
        {
            return Results.NotFound("Error: Verses not found with that keyword");
        }
        
        return Results.Ok(verses);
    }

    [HttpGet("{translation_id}/{book_id}/keywords/{keywords}")]
    public async Task<IResult> GetVersesInBookFilteredByKeywords(string translation_id, string book_id, string keywords)
    {
        var parsedKeyword = keywords.Replace('_',' ');
        List<string> keywordList = parsedKeyword.Split(',').ToList();
        if(keywordList.Count == 0){
            return Results.InternalServerError("Error: Bad input of keywords");
        }
        var verses = await _context.verses
        .Where(v => v.book_id == book_id && v.translation_id == translation_id && v.text.Contains(keywordList[0]))
        .ToListAsync();

        for (int i = 1; i < keywordList.Count; i++)
        {
            verses = [.. verses.Where(v => v.text.IndexOf(keywordList[i], StringComparison.OrdinalIgnoreCase) >= 0)];
        }

        if (verses.Count==0)
        {
            return Results.NotFound("Error: Verses not found with that keyword");
        }
        return Results.Ok(verses);
    }

    [HttpGet("translations")]
    public async Task<IResult> GetTranslations()
    {
        var translations = await _context.translations.ToListAsync();

        if (translations.Count==0)
        {
            return Results.NotFound("Error: Chapter not found");
        }
        return Results.Ok(translations);
    }

    [HttpGet("translations/{id}")]
    public async Task<IResult> GetTranslationsById(string id)
    {
        var translations = await _context.translations.Where(t => t.identifier == id).ToListAsync();

        if (translations.Count==0)
        {
            return Results.NotFound("Error: Chapter not found");
        }
        return Results.Ok(translations);
    }
    
}