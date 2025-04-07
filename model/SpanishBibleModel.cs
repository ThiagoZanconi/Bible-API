public class BookSpanish()
{
    public required string id { get; set; }
    public required string bibleId { get; set; }
    public required string abbreviation { get; set; }
    public required string name { get; set; } 
    public required string nameLong { get; set; }
}

public class BooksResponse
{
    public required List<BookSpanish>? data { get; set; }
}