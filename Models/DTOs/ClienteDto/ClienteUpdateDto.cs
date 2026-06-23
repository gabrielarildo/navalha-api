using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.ClienteDto
{
    public class ClienteUpdateDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }
    }
}
