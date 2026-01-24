using System.ComponentModel.DataAnnotations.Schema;

namespace MiProyectoBackend.postgres_model
{

    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("email")]
        public string Email { get; set; } = null!;
        [Column("password_hash")]
        public string Password { get; set; } = null!;
        [Column("role")]
        public string Role { get; set; } = null!;
    }

    public class LoginCredentials(string Email, string Password)
    {    
        public string Email { get; set; } = Email;
        public string Password { get; set; } = Password;
    }

    public class RegisterCredentials(string Name, string Email, string Password){
        public string Name { get; set; } = Name;
        public string Email { get; set; } = Email;
        public string Password { get; set; } = Password;
    }
}
