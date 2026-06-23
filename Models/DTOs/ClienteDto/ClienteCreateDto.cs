using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.ClienteDto
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [MinLength(3), MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O e-mail deve ser válido.")]
        public string? Email { get; set; }
    }
}
