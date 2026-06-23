using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.ClienteDto
{
    public static class ClienteMapper
    {
        public static ClienteResponseDto ToResponse(this Cliente c) => new()
        {
            Id = c.Id,
            Nome = c.Nome,
            Telefone = c.Telefone,
            Email = c.Email,
            CriadoEm = c.CriadoEm
        };

        public static Cliente ToEntity(this ClienteCreateDto dto) => new()
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Telefone = dto.Telefone.Trim(),
            Email = dto.Email?.Trim().ToLower(),
            CriadoEm = DateTime.UtcNow
        };
    }
}
