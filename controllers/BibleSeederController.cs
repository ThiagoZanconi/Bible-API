using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.database;
using MiProyectoBackend.model;
using System.Net.Http.Headers;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class BibleSeederController(HttpClient httpClient, AppDbContext context) : ControllerBase
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly AppDbContext _context = context;
    private readonly DbInsert _dbInsert = new DbInsert(context);
    private string API_KEY = "b1a40be841de7af15b6b029fa873afe2";
    private string API_LINK = "https://api.scripture.api.bible/v1/bibles/b32b9d1b64b4ef29-01/";

    [HttpGet("books")]
    public async Task<List<BookSpanish>?> GetBooksAsync(){
            var request = new HttpRequestMessage(HttpMethod.Get, API_LINK+"books");
            request.Headers.Add("api-key", API_KEY);
            var response = await _httpClient.SendAsync(request);
            try{
                if (response.IsSuccessStatusCode)
                {
                    
                    string json =  await response.Content.ReadAsStringAsync();
                    BooksResponse? res = JsonSerializer.Deserialize<BooksResponse>(json);
                    return res?.data ?? null;
     
                }
            }
            catch(Exception e){
                Console.WriteLine("Error: "+e.Message);
            }
            return null;
    }

    [HttpGet("books/{book_id}/chapters")]
    public async Task<List<ChapterSpanish>?> GetChaptersAsync(string book_id){
            var request = new HttpRequestMessage(HttpMethod.Get, API_LINK+"books/"+book_id+"/chapters");
            request.Headers.Add("api-key", API_KEY);
            var response = await _httpClient.SendAsync(request);
            try{
                if (response.IsSuccessStatusCode)
                {
                    string json =  await response.Content.ReadAsStringAsync();
                    ChaptersResponse? res = JsonSerializer.Deserialize<ChaptersResponse>(json);
                    return res?.data ?? null;
                }
            }
            catch(Exception e){
                Console.WriteLine("Error: "+e.Message);
            }
            return null;
    }

    [HttpGet("books/{book_id}/chapters/{chapterId}/verses")]
    public async Task<List<VerseSpanish>?> GetVersesAsync(string book_id, string chapterId){
            var request = new HttpRequestMessage(HttpMethod.Get, API_LINK+"books/"+book_id+"/chapters/"+chapterId+"/verses");
            request.Headers.Add("api-key", API_KEY);
            var response = await _httpClient.SendAsync(request);
            try{
                if (response.IsSuccessStatusCode)
                {
                    string json =  await response.Content.ReadAsStringAsync();
                    VersesResponse? res = JsonSerializer.Deserialize<VersesResponse>(json);
                    return res?.data ?? null;
                }
            }
            catch(Exception e){
                Console.WriteLine("Error: "+e.Message);
            }
            return null;
    }

    [HttpGet("verse/{verse_id}")]
    public async Task<Data?> GetVerseAsync(string verse_id){
        string content_type = "text";
        string url = $"{API_LINK}verses/{verse_id}?content-type={content_type}&include-verse-numbers=false";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("api-key", API_KEY);
        var response = await _httpClient.SendAsync(request);
        try{
            if (response.IsSuccessStatusCode)
            {
                string json =  await response.Content.ReadAsStringAsync();
                ApiResponse? apiResponse = JsonSerializer.Deserialize<ApiResponse>(json);
                if(apiResponse!=null){
                    apiResponse.data.content = apiResponse.data.content.TrimStart();
                }
                return apiResponse?.data;
            }
        }
        catch(Exception e){
            Console.WriteLine("Error: "+e.Message);
        }
        return null;
    }
    /*
    [HttpPost("translation")]
    public async Task<IResult> PostTranslation(){
        try{
            Translation translation = new("BES","La Biblia en Español Sencillo","Español","esp","Creative Commons Reconocimiento");
            _dbInsert.InsertTranslation(translation);
            return Results.Ok("Traduccion agregada correctamente");
        }
        catch(Exception e){
            Console.WriteLine("Error: "+e.Message);
        }
        return Results.InternalServerError("Traduccion no agregada correctamente");
    }
    */
    [HttpPost("books/{id}")]
    public async Task<IResult> PostBook(string id){
        try{

            var chapters = await _context.chapters.Where(c => c.book_id == id).ToListAsync();
            foreach(var chapter in chapters){
                var verses = await _context.verses.Where(v => v.chapter == chapter.chapter && v.book_id==id).ToListAsync();
                foreach(var verse in verses){
                    Data? data = await GetVerseAsync(id+"."+verse.chapter+"."+verse.verse);
                    
                    if(data!=null){
                        string text = data.content;
                        Verse input = new(verse.book_id, verse.book, verse.chapter, verse.verse, text, "BES");
                        _dbInsert.InsertVerse(input);
                        Console.WriteLine(data.id);
                        await Task.Delay(100);
                    }
                    
                }
                
            }
            return Results.Ok("Versos seedeados correctamente");
        }catch(Exception e){
            Console.WriteLine("Error: "+e.Message);
        }
        return Results.InternalServerError("Error: No se seedearon correctamente los versos");
        
    }
}