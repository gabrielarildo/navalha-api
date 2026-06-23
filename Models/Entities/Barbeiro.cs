namespace TodoApi.Models.Entities
{
    public class Barbeiro
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Especialidade { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        // Relação: 1 Barbeiro pode ter N agendamentos
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
