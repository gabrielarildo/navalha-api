using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.DTOs.ServicoDto;

namespace TodoApi.Services
{
    public class ServicoService(AppDbContext context)
    {
        public async Task<List<ServicoResponseDto>> GetAllAsync()
        {
            var servicos = await context.Servicos
                .AsNoTracking()
                .OrderBy(s => s.Nome)
                .ToListAsync();

            return servicos.Select(s => s.ToResponse()).ToList();
        }

        public async Task<ServicoResponseDto?> GetByIdAsync(Guid id)
        {
            var servico = await context.Servicos
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            return servico?.ToResponse();
        }

        public async Task<ServicoResponseDto> CreateAsync(ServicoCreateDto dto)
        {
            var servico = dto.ToEntity();

            context.Servicos.Add(servico);
            await context.SaveChangesAsync();

            return servico.ToResponse();
        }

        public async Task<ServicoResponseDto?> UpdateAsync(Guid id, ServicoUpdateDto dto)
        {
            var servico = await context.Servicos.FirstOrDefaultAsync(s => s.Id == id);
            if (servico is null) return null;

            servico.Nome = dto.Nome.Trim();
            servico.Descricao = dto.Descricao?.Trim();
            servico.Valor = dto.Valor;
            servico.DuracaoMinutos = dto.DuracaoMinutos;
            servico.Ativo = dto.Ativo;

            await context.SaveChangesAsync();
            return servico.ToResponse();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var servico = await context.Servicos.FirstOrDefaultAsync(s => s.Id == id);
            if (servico is null) return false;

            context.Servicos.Remove(servico);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
