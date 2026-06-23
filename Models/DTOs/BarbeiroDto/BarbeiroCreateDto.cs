using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.BarbeiroDto
{
    public class BarbeiroCreateDto
    {
        [Required(ErrorMessage = "O nome do barbeiro é obrigatório.")]
        [MinLength(3), MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Especialidade { get; set; }
    }
}
