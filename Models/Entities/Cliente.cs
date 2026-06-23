namespace TodoApi.Models.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        // Relação: 1 Cliente pode ter N agendamentos (histórico de atendimentos)
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
