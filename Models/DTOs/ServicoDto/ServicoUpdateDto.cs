using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.ServicoDto
{
    public class ServicoUpdateDto
    {
        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Descricao { get; set; }

        [Required, Range(0.01, 10000)]
        public decimal Valor { get; set; }

        [Required, Range(5, 480)]
        public int DuracaoMinutos { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
