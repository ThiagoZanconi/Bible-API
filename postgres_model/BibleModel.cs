using System.ComponentModel.DataAnnotations.Schema;

namespace MiProyectoBackend.postgres_model{
    [Table("translations")]
    public class Translation
    {
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("language")]   
        public string Language { get; set; } = null!;
        [Column("language_code")]
        public string LanguageCode { get; set; } = null!;
        [Column("license")]
        public string License { get; set; } = null!;
    }

    [Table("books")]
    public class Book
    {
        [Column("id")]
        public string Id { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("translation_id")]
        public string TranslationId { get; set; } = null!;
    }

    [Table("chapters")]
    public class Chapter
    {
        [Column("book_id")]
        public string BookId { get; set; } = null!;
        [Column("chapter")]
        public int Chp{ get; set; } = 0;
        [Column("translation_id")]

        public string TranslationId { get; set; } = null!;
    }

    [Table("verses")]
    public class Verse
    {
        [Column("book_id")]
        public string BookId { get; set; } = null!;
        [Column("chapter")]
        public int Chapter { get; set; } = 0;
        [Column("verse")]
        public int Vrs { get; set; } = 0;
        [Column("text")]
        public string Text { get; set; } = null!;
        [Column("translation_id")]
        public string TranslationId { get; set; } = null!;
    }

    public class KeywordSearch
    {
        public int Index { get; set; } = 0;
        public int MaxIndex { get; set; } = 0;
        public List<Verse> Verses { get; set; } = new List<Verse>();
    }

}