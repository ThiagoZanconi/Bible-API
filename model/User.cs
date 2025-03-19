namespace MiProyectoBackend.model
{
    public class User(string Name, string Email, string Password, string Role)
    {
        public int Id { get; set; }
        public string Name { get; set; } = Name;
        public string Email { get; set; } = Email;
        public string Password { get; set; } = Password;
        public string Role { get; set; } = Role;
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
