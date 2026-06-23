using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.AgendamentoDto
{
    public class AgendamentoCreateDto
    {
        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "O barbeiro é obrigatório.")]
        public Guid BarbeiroId { get; set; }

        [Required(ErrorMessage = "O serviço é obrigatório.")]
        public Guid ServicoId { get; set; }

        [Required(ErrorMessage = "A data e o horário do agendamento são obrigatórios.")]
        public DateTime DataHora { get; set; }

        [MaxLength(300)]
        public string? Observacoes { get; set; }
    }
}
