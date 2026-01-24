using Microsoft.EntityFrameworkCore;

namespace MiProyectoBackend.postgres_model
{
    public class PostgresContext : DbContext
    {
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Verse> Verses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Collection> Collections {get; set;}
        public DbSet<VerseCollection> VerseCollection {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => new { u.Id });

            modelBuilder.Entity<Collection>()
                .HasKey(c => new { c.Id });

            modelBuilder.Entity<Translation>()
                .HasKey(t => new { t.Id });

            modelBuilder.Entity<Book>()
                .HasKey(b => new { b.Id, b.TranslationId });

            modelBuilder.Entity<Chapter>()
                .HasKey(c => new { c.BookId, c.TranslationId, c.Chp });

            modelBuilder.Entity<Verse>()
                .HasKey(v => new { v.BookId, v.Chapter, v.Vrs, v.TranslationId });

            modelBuilder.Entity<VerseCollection>()
                .HasKey(c => new { c.BookId, c.Chapter, c.Verse, c.CollectionId });
        }
    }
}