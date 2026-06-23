using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.ServicoDto
{
    public class ServicoCreateDto
    {
        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O valor do serviço é obrigatório.")]
        [Range(0.01, 10000, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A duração do serviço é obrigatória.")]
        [Range(5, 480, ErrorMessage = "A duração deve estar entre 5 e 480 minutos.")]
        public int DuracaoMinutos { get; set; }
    }
}
