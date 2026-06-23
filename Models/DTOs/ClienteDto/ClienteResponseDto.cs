namespace TodoApi.Models.DTOs.ClienteDto
{
    public class ClienteResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
