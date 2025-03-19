using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.model;

namespace MiProyectoBackend.database
{
    public class DbInsert(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        public void InsertTranslation(Translation translation){
            try{
                _context.translations.Add(translation);
                _context.SaveChanges();
                Console.WriteLine("Translation added succesfully");
            }
            catch (DbUpdateException ex)
            {
                // Verificamos si la excepción es causada por una violación de clave primaria (duplicada)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY KEY"))
                {
                    // Aquí puedes manejar el caso cuando ya existe una translation con esa primary key
                    Console.WriteLine("Ya existe una traducción con esa clave primaria.");
                }
                else
                {
                    // Si la excepción no es por violación de clave primaria, la manejas de otra manera
                    Console.WriteLine($"Error al agregar la traducción: {ex.Message}");
                }
            } 
        }

        public void InsertBook(Book book){
            try{
                _context.books.Add(book);
                _context.SaveChanges();
                Console.WriteLine("Book added succesfully");
            }
            catch (DbUpdateException ex)
            {
                // Verificamos si la excepción es causada por una violación de clave primaria (duplicada)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY KEY"))
                {
                    // Aquí puedes manejar el caso cuando ya existe una translation con esa primary key
                    Console.WriteLine("Ya existe un book con esa clave primaria.");
                }
                else
                {
                    // Si la excepción no es por violación de clave primaria, la manejas de otra manera
                    Console.WriteLine($"Error al agregar el book: {ex.Message}");
                }
            } 
        }

        public void InsertChapter(Chapter chapter){
            try{
                _context.chapters.Add(chapter);
                _context.SaveChanges();
                Console.WriteLine("Chapter added succesfully");
            }
            catch (DbUpdateException ex)
            {
                // Verificamos si la excepción es causada por una violación de clave primaria (duplicada)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY KEY"))
                {
                    // Aquí puedes manejar el caso cuando ya existe una translation con esa primary key
                    Console.WriteLine("Ya existe un chapter con esa clave primaria.");
                }
                else
                {
                    // Si la excepción no es por violación de clave primaria, la manejas de otra manera
                    Console.WriteLine($"Error al agregar el chapter: {ex.Message}");
                }
            } 
        }

        public void InsertVerse(Verse verse){
            try{
                _context.verses.Add(verse);
                _context.SaveChanges();
                Console.WriteLine("Verse added succesfully");
            }
            catch (DbUpdateException ex)
            {
                // Verificamos si la excepción es causada por una violación de clave primaria (duplicada)
                if (ex.InnerException != null && ex.InnerException.Message.Contains("PRIMARY KEY"))
                {
                    // Aquí puedes manejar el caso cuando ya existe una translation con esa primary key
                    Console.WriteLine("Ya existe un verso con esa clave primaria.");
                }
                else
                {
                    // Si la excepción no es por violación de clave primaria, la manejas de otra manera
                    Console.WriteLine($"Error al agregar el verso: {ex.Message}");
                }
            } 
        }
    }
}