namespace TodoApi.Models.Entities
{
    public class Agendamento
    {
        public Guid Id { get; set; }

        // fk - 1 Cliente pode ter N agendamentos
        public Guid ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // fk - 1 Barbeiro pode ter N agendamentos
        public Guid BarbeiroId { get; set; }
        public Barbeiro? Barbeiro { get; set; }

        // fk - 1 Serviço pode estar em N agendamentos
        public Guid ServicoId { get; set; }
        public Servico? Servico { get; set; }

        // Data e horário combinados do atendimento
        public DateTime DataHora { get; set; }

        public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;

        // Valor calculado automaticamente a partir do Serviço no momento da criação
        public decimal Valor { get; set; }

        public string? Observacoes { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
    }
}
