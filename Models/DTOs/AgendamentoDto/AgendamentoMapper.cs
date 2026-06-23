using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.AgendamentoDto
{
    public static class AgendamentoMapper
    {
        public static AgendamentoResponseDto ToResponse(this Agendamento a) => new()
        {
            Id = a.Id,
            ClienteId = a.ClienteId,
            ClienteNome = a.Cliente?.Nome,
            BarbeiroId = a.BarbeiroId,
            BarbeiroNome = a.Barbeiro?.Nome,
            ServicoId = a.ServicoId,
            ServicoNome = a.Servico?.Nome,
            DuracaoMinutos = a.Servico?.DuracaoMinutos ?? 0,
            DataHora = a.DataHora,
            Status = a.Status,
            Valor = a.Valor,
            Observacoes = a.Observacoes,
            CriadoEm = a.CriadoEm,
            AtualizadoEm = a.AtualizadoEm
        };

        public static Agendamento ToEntity(this AgendamentoCreateDto dto, decimal valorServico) => new()
        {
            Id = Guid.NewGuid(),
            ClienteId = dto.ClienteId,
            BarbeiroId = dto.BarbeiroId,
            ServicoId = dto.ServicoId,
            DataHora = dto.DataHora,
            Observacoes = dto.Observacoes?.Trim(),
            Valor = valorServico,
            Status = StatusAgendamento.Agendado,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };
    }
}
