using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.BarbeiroDto
{
    public static class BarbeiroMapper
    {
        public static BarbeiroResponseDto ToResponse(this Barbeiro b) => new()
        {
            Id = b.Id,
            Nome = b.Nome,
            Telefone = b.Telefone,
            Especialidade = b.Especialidade,
            Ativo = b.Ativo,
            CriadoEm = b.CriadoEm
        };

        public static Barbeiro ToEntity(this BarbeiroCreateDto dto) => new()
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Telefone = dto.Telefone.Trim(),
            Especialidade = dto.Especialidade?.Trim(),
            Ativo = true,
            CriadoEm = DateTime.UtcNow
        };
    }
}
