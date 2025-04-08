public class BookSpanish()
{
    public required string id { get; set; }
    public required string bibleId { get; set; }
    public required string abbreviation { get; set; }
    public required string name { get; set; } 
    public required string nameLong { get; set; }
}

public class ChapterSpanish(){
    public required string id {get; set;}
    public required string bibleId { get; set; }
    public required string bookId { get; set; }
    public required string number { get; set; } 
    public required string reference { get; set; }
}


public class VerseSpanish(){
    public required string id {get; set;}
    public required string orgId { get; set; }
    public required string bookId { get; set; }
    public required string chapterId { get; set; } 
    public required string bibleId { get; set; }
    public required string reference { get; set; }
}

public class VersesResponse
{
    public required List<VerseSpanish>? data { get; set; }
}

public class ChaptersResponse
{
    public required List<ChapterSpanish>? data { get; set; }
}

public class BooksResponse
{
    public required List<BookSpanish>? data { get; set; }
}

public class ApiResponse
{
    public required Data data { get; set; }
}

public class Data
{
    public required string id {get; set;}
    public required string orgId { get; set; }
    public required string bookId { get; set; }
    public required string chapterId { get; set; } 
    public required string bibleId { get; set; }
    public required string reference { get; set; }
    public required string content { get; set; }
    public required int verseCount { get; set; }
    public required string copyright { get; set; }
    public required AdyacentVerse next {get; set;}
    public required AdyacentVerse previous {get; set;}
}

public class AdyacentVerse{
    public required string id {get; set;}
    public required string number { get; set; }
}
