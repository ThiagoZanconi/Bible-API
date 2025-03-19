using Microsoft.EntityFrameworkCore;
using MiProyectoBackend.model;

namespace MiProyectoBackend.database
{

    public class VerseSeeder(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public void SeedTranslation(){
            var translation = new Translation("kjv","King James Version","English","eng","Public Domain");

            try{
                _context.translations.Add(translation);
                _context.SaveChanges();
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
    }
}