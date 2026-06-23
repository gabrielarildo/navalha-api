namespace TodoApi.Models.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        
        public string Nome { get;set; } = string.Empty;
        public string Email { get;set; } = string.Empty;
        public string PasswordHash { get;set; } = string.Empty;

        // Papel do usuário no sistema: "Admin", "Barbeiro" ou "Cliente"
        public string Role { get; set; } = "Cliente";
    }
}