using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.AgendamentoDto
{
    // Usado para reagendar (alterar data/horário/serviço/observações) um agendamento existente
    public class AgendamentoUpdateDto
    {
        [Required(ErrorMessage = "O serviço é obrigatório.")]
        public Guid ServicoId { get; set; }

        [Required(ErrorMessage = "A data e o horário do agendamento são obrigatórios.")]
        public DateTime DataHora { get; set; }

        [MaxLength(300)]
        public string? Observacoes { get; set; }
    }
}
