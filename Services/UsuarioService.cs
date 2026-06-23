using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.DTOs.UsuarioDto;

namespace TodoApi.Services
{
    public class UsuarioService(AppDbContext context)
    {
        // Serviço que busca todos os usuários
        public async Task<List<UsuarioResponseDto>> GetAllAsync()
        {
            var usuarios = await context.Usuarios
            .AsNoTracking()
            .ToListAsync();

            return usuarios.Select(u => u.ToResponse()).ToList();
        }

        // Serviço que retorna um usuário pelo ID
        public async Task<UsuarioResponseDto> GetByIdAsync(Guid id)
        {
            var usuario = await context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            return usuario?.ToResponse();
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
        {
            // Verificar se o email existe
            var emailExiste = await context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
            {
                throw new Exception("Este e-mail já está cadastrado!");
            }

            // Converte o DTO de criação para a entidade/tabela
            var usuario = dto.ToEntity();

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            // Retorna o usuário já criado no formato de resposta
            return usuario.ToResponse();
        }    
    }
}