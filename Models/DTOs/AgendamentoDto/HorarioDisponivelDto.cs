namespace TodoApi.Models.DTOs.AgendamentoDto
{
    // Representa um slot de horário e se ele está livre para um determinado barbeiro/data
    public class HorarioDisponivelDto
    {
        public TimeSpan Horario { get; set; }
        public bool Disponivel { get; set; }
    }
}
