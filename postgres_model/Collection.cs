using System.ComponentModel.DataAnnotations.Schema;

namespace MiProyectoBackend.postgres_model{

    [Table("collections")]
     public class Collection
    {
        [Column("id")]
        public int Id { get; set; }              // AUTO_INCREMENT
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    [Table("verse_collections")]
    public class VerseCollection
    {

        [Column("collection_id")]
        public int CollectionId { get; set; }
        [Column("book_id")]
        public string BookId { get; set; } = null!;
        [Column("chapter")]
        public int Chapter { get; set; }
        [Column("verse")]
        public int Verse { get; set; }
    }

}