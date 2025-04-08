using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.model;

namespace MiProyectoBackend.database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Translation> translations { get; set; }
        public DbSet<Verse> verses { get; set; }
        public DbSet<Chapter> chapters { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Collection> collections {get; set;}
        public DbSet<Verse_Collection> verse_collection {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>()
                .HasKey(c => new { c.book_id, c.chapter });

            modelBuilder.Entity<Translation>()
                .HasKey(t => new { t.identifier });

            modelBuilder.Entity<Book>()
                .HasKey(b => new { b.id });

            modelBuilder.Entity<Verse>()
                .HasKey(v => new { v.book_id, v.chapter, v.verse, v.translation_id });

            modelBuilder.Entity<Collection>()
                .HasKey(c => new { c.name });

            modelBuilder.Entity<Verse_Collection>()
                .HasKey(c => new { c.collection_name, c.book_id, c.chapter, c.verse });
        }
    }
}