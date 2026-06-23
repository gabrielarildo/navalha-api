using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.BarbeiroDto
{
    public class BarbeiroUpdateDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Especialidade { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
