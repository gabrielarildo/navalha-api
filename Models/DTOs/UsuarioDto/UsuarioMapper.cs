using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.UsuarioDto
{
    public static class UsuarioMapper
    {
        public static UsuarioResponseDto ToResponse(this Usuario u) => new()
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Role = u.Role
        };

        public static Usuario ToEntity(this UsuarioCreateDto dto) => new()
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim().ToLower(),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "Cliente" : dto.Role.Trim()
        };
    }
}