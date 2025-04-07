using Microsoft.AspNetCore.Mvc;
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
    //private readonly DbInsert _dbInsert = new DbInsert(context);
    private string API_KEY = "b1a40be841de7af15b6b029fa873afe2";
    private string API_LINK = "https://api.scripture.api.bible/v1/bibles/48acedcf8595c754-01/";

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

    private async Task PrintVersesAsync(string link){
        await Task.Delay(2000);
        HttpResponseMessage response = await _httpClient.GetAsync(link);
        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Link: "+link);
        Console.WriteLine("Response Body: "+responseBody);

        while((""+responseBody).Equals("Retry Later")){
            await Task.Delay(5000);
            response = await _httpClient.GetAsync(link);
            responseBody = await response.Content.ReadAsStringAsync();
        }

        using JsonDocument doc = JsonDocument.Parse(responseBody);
        JsonElement root = doc.RootElement;
        JsonElement verses = root.GetProperty("verses");
        
        foreach (JsonElement v in verses.EnumerateArray())
        {
            int chapter = v.GetProperty("chapter").GetInt32()!;
            string book_id = v.GetProperty("book_id").GetString()!;
            string book = v.GetProperty("book").GetString()!;
            int verse = v.GetProperty("verse").GetInt32()!;
            string text = v.GetProperty("text").GetString()!;

            Chapter? chap = _context.chapters.FirstOrDefault(c => c.book_id == book_id && c.chapter == chapter);
            if(chap==null){
                //_dbInsert.InsertChapter(new Chapter(book_id,book,chapter));
            }

            //_dbInsert.InsertVerse(new Verse(book_id,book,chapter,verse,text,"kjv"));
        }
    }
}