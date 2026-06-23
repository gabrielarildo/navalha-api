using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.ServicoDto
{
    public static class ServicoMapper
    {
        public static ServicoResponseDto ToResponse(this Servico s) => new()
        {
            Id = s.Id,
            Nome = s.Nome,
            Descricao = s.Descricao,
            Valor = s.Valor,
            DuracaoMinutos = s.DuracaoMinutos,
            Ativo = s.Ativo
        };

        public static Servico ToEntity(this ServicoCreateDto dto) => new()
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Descricao = dto.Descricao?.Trim(),
            Valor = dto.Valor,
            DuracaoMinutos = dto.DuracaoMinutos,
            Ativo = true
        };
    }
}
