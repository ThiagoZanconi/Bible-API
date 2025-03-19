namespace MiProyectoBackend.model{
    public class Translation(string identifier, string name, string language, string languageCode, string license)
    {
        public string identifier { get; set; } = identifier;
        public string name { get; set; } = name;   
        public string language { get; set; } = language;
        public string languageCode { get; set; } = languageCode;
        public string license { get; set; } = license;
    }

    public class Book(string id, string name, int number)
    {
        public string id { get; set; } = id;
        public string name { get; set; } = name;
        public int number { get; set; } = number;
    }

    public class Chapter(string book_id, string book, int chapter)
    {
        public string book_id { get; set; } = book_id;
        public string book { get; set; } = book;
        public int chapter { get; set; } = chapter;
    }

    public class Verse(string book_id, string book, int chapter, int verse, string text, string translation_id)
    {
       
        public string book_id { get; set; } = book_id;
        public int chapter { get; set; } = chapter;
        public string book { get; set; } = book;
        public int verse { get; set; } = verse;
        public string text { get; set; } = text;
        public string translation_id { get; set; } = translation_id;
    }

}