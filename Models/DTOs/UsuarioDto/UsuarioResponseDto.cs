namespace TodoApi.Models.DTOs.UsuarioDto
//Caminho de onde está o arquivo
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}