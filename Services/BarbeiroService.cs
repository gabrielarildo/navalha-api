using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.DTOs.AgendamentoDto;
using TodoApi.Models.DTOs.BarbeiroDto;
using TodoApi.Models.Entities;

namespace TodoApi.Services
{
    public class BarbeiroService(AppDbContext context)
    {
        // Horário de funcionamento padrão da barbearia Navalha
        private static readonly TimeSpan AberturaPadrao = new(9, 0, 0);
        private static readonly TimeSpan FechamentoPadrao = new(19, 0, 0);
        private static readonly TimeSpan IntervaloPadrao = TimeSpan.FromMinutes(30);

        public async Task<List<BarbeiroResponseDto>> GetAllAsync()
        {
            var barbeiros = await context.Barbeiros
                .AsNoTracking()
                .OrderBy(b => b.Nome)
                .ToListAsync();

            return barbeiros.Select(b => b.ToResponse()).ToList();
        }

        public async Task<BarbeiroResponseDto?> GetByIdAsync(Guid id)
        {
            var barbeiro = await context.Barbeiros
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            return barbeiro?.ToResponse();
        }

        public async Task<BarbeiroResponseDto> CreateAsync(BarbeiroCreateDto dto)
        {
            var barbeiro = dto.ToEntity();

            context.Barbeiros.Add(barbeiro);
            await context.SaveChangesAsync();

            return barbeiro.ToResponse();
        }

        public async Task<BarbeiroResponseDto?> UpdateAsync(Guid id, BarbeiroUpdateDto dto)
        {
            var barbeiro = await context.Barbeiros.FirstOrDefaultAsync(b => b.Id == id);
            if (barbeiro is null) return null;

            barbeiro.Nome = dto.Nome.Trim();
            barbeiro.Telefone = dto.Telefone.Trim();
            barbeiro.Especialidade = dto.Especialidade?.Trim();
            barbeiro.Ativo = dto.Ativo;

            await context.SaveChangesAsync();
            return barbeiro.ToResponse();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var barbeiro = await context.Barbeiros.FirstOrDefaultAsync(b => b.Id == id);
            if (barbeiro is null) return false;

            context.Barbeiros.Remove(barbeiro);
            await context.SaveChangesAsync();
            return true;
        }

        // Regra: retorna os horários disponíveis de um barbeiro em uma data específica,
        // considerando os agendamentos já existentes (Agendado ou Confirmado) e a duração
        // padrão de atendimento (slots de 30 minutos dentro do horário de funcionamento).
        public async Task<List<HorarioDisponivelDto>> GetHorariosDisponiveisAsync(Guid barbeiroId, DateOnly data)
        {
            var inicioDoDia = data.ToDateTime(TimeOnly.MinValue);
            var fimDoDia = inicioDoDia.AddDays(1);

            var agendamentosDoDia = await context.Agendamentos
                .Include(a => a.Servico)
                .Where(a => a.BarbeiroId == barbeiroId
                    && a.DataHora >= inicioDoDia
                    && a.DataHora < fimDoDia
                    && a.Status != StatusAgendamento.Cancelado)
                .AsNoTracking()
                .ToListAsync();

            var horarios = new List<HorarioDisponivelDto>();

            for (var horario = AberturaPadrao; horario < FechamentoPadrao; horario += IntervaloPadrao)
            {
                var inicioSlot = inicioDoDia + horario;

                var ocupado = agendamentosDoDia.Any(a =>
                {
                    var duracao = TimeSpan.FromMinutes(a.Servico?.DuracaoMinutos ?? 30);
                    var fimAgendamento = a.DataHora + duracao;
                    return inicioSlot < fimAgendamento && inicioSlot + IntervaloPadrao > a.DataHora;
                });

                horarios.Add(new HorarioDisponivelDto
                {
                    Horario = horario,
                    Disponivel = !ocupado && inicioSlot > DateTime.UtcNow
                });
            }

            return horarios;
        }
    }
}
