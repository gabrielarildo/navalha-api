namespace TodoApi.Models.Entities
{
    public class Servico
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        // Valor padrão do serviço (usado para calcular o valor do agendamento automaticamente)
        public decimal Valor { get; set; }

        // Duração estimada em minutos, usada para calcular disponibilidade de horários
        public int DuracaoMinutos { get; set; }

        public bool Ativo { get; set; } = true;

        // Relação: 1 Serviço pode estar em N agendamentos
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
