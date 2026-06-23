using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.DTOs.AgendamentoDto;
using TodoApi.Models.DTOs.ClienteDto;

namespace TodoApi.Services
{
    public class ClienteService(AppDbContext context)
    {
        public async Task<List<ClienteResponseDto>> GetAllAsync()
        {
            var clientes = await context.Clientes
                .AsNoTracking()
                .OrderBy(c => c.Nome)
                .ToListAsync();

            return clientes.Select(c => c.ToResponse()).ToList();
        }

        public async Task<ClienteResponseDto?> GetByIdAsync(Guid id)
        {
            var cliente = await context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return cliente?.ToResponse();
        }

        public async Task<ClienteResponseDto> CreateAsync(ClienteCreateDto dto)
        {
            var cliente = dto.ToEntity();

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            return cliente.ToResponse();
        }

        public async Task<ClienteResponseDto?> UpdateAsync(Guid id, ClienteUpdateDto dto)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente is null) return null;

            cliente.Nome = dto.Nome.Trim();
            cliente.Telefone = dto.Telefone.Trim();
            cliente.Email = dto.Email?.Trim().ToLower();

            await context.SaveChangesAsync();
            return cliente.ToResponse();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente is null) return false;

            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();
            return true;
        }

        // Histórico de atendimentos do cliente
        public async Task<List<AgendamentoResponseDto>> GetHistoricoAsync(Guid clienteId)
        {
            var agendamentos = await context.Agendamentos
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Where(a => a.ClienteId == clienteId)
                .OrderByDescending(a => a.DataHora)
                .AsNoTracking()
                .ToListAsync();

            return agendamentos.Select(a => a.ToResponse()).ToList();
        }
    }
}
