using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.AgendamentoDto
{
    public class AgendamentoResponseDto
    {
        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }
        public string? ClienteNome { get; set; }

        public Guid BarbeiroId { get; set; }
        public string? BarbeiroNome { get; set; }

        public Guid ServicoId { get; set; }
        public string? ServicoNome { get; set; }
        public int DuracaoMinutos { get; set; }

        public DateTime DataHora { get; set; }
        public StatusAgendamento Status { get; set; }
        public decimal Valor { get; set; }
        public string? Observacoes { get; set; }

        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
