namespace TodoApi.Models.DTOs.ServicoDto
{
    public class ServicoResponseDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; }
    }
}
