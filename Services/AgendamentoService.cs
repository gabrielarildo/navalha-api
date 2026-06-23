using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models.DTOs.AgendamentoDto;
using TodoApi.Models.Entities;

namespace TodoApi.Services
{
    public class AgendamentoService(AppDbContext context)
    {
        public async Task<List<AgendamentoResponseDto>> GetAllAsync()
        {
            var agendamentos = await context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .OrderBy(a => a.DataHora)
                .AsNoTracking()
                .ToListAsync();

            return agendamentos.Select(a => a.ToResponse()).ToList();
        }

        public async Task<AgendamentoResponseDto?> GetByIdAsync(Guid id)
        {
            var agendamento = await BuscarComIncludesAsync(id);
            return agendamento?.ToResponse();
        }

        // Regra: cria o agendamento validando data futura, existência das entidades
        // relacionadas e disponibilidade do barbeiro no horário solicitado. O valor é
        // calculado automaticamente a partir do preço cadastrado para o serviço.
        public async Task<AgendamentoResponseDto> CreateAsync(AgendamentoCreateDto dto)
        {
            if (dto.DataHora <= DateTime.UtcNow)
                throw new InvalidOperationException("Não é permitido agendar em uma data/horário no passado.");

            var cliente = await context.Clientes.FindAsync(dto.ClienteId)
                ?? throw new InvalidOperationException("Cliente não encontrado.");

            var barbeiro = await context.Barbeiros.FindAsync(dto.BarbeiroId)
                ?? throw new InvalidOperationException("Barbeiro não encontrado.");
            if (!barbeiro.Ativo)
                throw new InvalidOperationException("Este barbeiro não está ativo para novos agendamentos.");

            var servico = await context.Servicos.FindAsync(dto.ServicoId)
                ?? throw new InvalidOperationException("Serviço não encontrado.");
            if (!servico.Ativo)
                throw new InvalidOperationException("Este serviço não está disponível.");

            await GarantirHorarioDisponivelAsync(dto.BarbeiroId, dto.DataHora, servico.DuracaoMinutos);

            // Valor calculado automaticamente a partir do serviço escolhido
            var agendamento = dto.ToEntity(servico.Valor);

            context.Agendamentos.Add(agendamento);
            await context.SaveChangesAsync();

            return (await BuscarComIncludesAsync(agendamento.Id))!.ToResponse();
        }

        // Regra: permite reagendar (mudar data/serviço/observações) desde que o
        // agendamento ainda não tenha sido concluído ou cancelado, a nova data não
        // esteja no passado e o novo horário esteja disponível para o barbeiro.
        public async Task<AgendamentoResponseDto?> UpdateAsync(Guid id, AgendamentoUpdateDto dto)
        {
            var agendamento = await context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id);
            if (agendamento is null) return null;

            if (agendamento.Status is StatusAgendamento.Concluido or StatusAgendamento.Cancelado)
                throw new InvalidOperationException("Não é possível alterar um agendamento concluído ou cancelado.");

            if (dto.DataHora <= DateTime.UtcNow)
                throw new InvalidOperationException("Não é permitido agendar em uma data/horário no passado.");

            var servico = await context.Servicos.FindAsync(dto.ServicoId)
                ?? throw new InvalidOperationException("Serviço não encontrado.");

            await GarantirHorarioDisponivelAsync(agendamento.BarbeiroId, dto.DataHora, servico.DuracaoMinutos, ignorarAgendamentoId: id);

            agendamento.ServicoId = dto.ServicoId;
            agendamento.DataHora = dto.DataHora;
            agendamento.Observacoes = dto.Observacoes?.Trim();
            agendamento.Valor = servico.Valor;
            agendamento.AtualizadoEm = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return (await BuscarComIncludesAsync(id))!.ToResponse();
        }

        // Regra: altera o status do agendamento (Confirmado, Concluído, Cancelado).
        // Cancelamento só é permitido antes do horário agendado.
        public async Task<AgendamentoResponseDto?> AtualizarStatusAsync(Guid id, StatusAgendamento novoStatus)
        {
            var agendamento = await context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id);
            if (agendamento is null) return null;

            if (agendamento.Status == StatusAgendamento.Cancelado)
                throw new InvalidOperationException("Este agendamento já está cancelado.");

            if (agendamento.Status == StatusAgendamento.Concluido)
                throw new InvalidOperationException("Este agendamento já foi concluído e não pode ser alterado.");

            if (novoStatus == StatusAgendamento.Cancelado && agendamento.DataHora <= DateTime.UtcNow)
                throw new InvalidOperationException("Não é possível cancelar um agendamento cujo horário já passou.");

            agendamento.Status = novoStatus;
            agendamento.AtualizadoEm = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return (await BuscarComIncludesAsync(id))!.ToResponse();
        }

        // Cancelamento explícito (atalho para AtualizarStatusAsync)
        public Task<AgendamentoResponseDto?> CancelarAsync(Guid id) =>
            AtualizarStatusAsync(id, StatusAgendamento.Cancelado);

        public async Task<List<AgendamentoResponseDto>> GetByBarbeiroAsync(Guid barbeiroId)
        {
            var agendamentos = await context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .Where(a => a.BarbeiroId == barbeiroId)
                .OrderBy(a => a.DataHora)
                .AsNoTracking()
                .ToListAsync();

            return agendamentos.Select(a => a.ToResponse()).ToList();
        }

        // Regra: não permitir agendamentos em horários já ocupados — verifica se existe
        // sobreposição entre o intervalo do novo agendamento e os intervalos de
        // agendamentos já existentes (não cancelados) para o mesmo barbeiro.
        private async Task GarantirHorarioDisponivelAsync(Guid barbeiroId, DateTime dataHora, int duracaoMinutos, Guid? ignorarAgendamentoId = null)
        {
            var inicioNovo = dataHora;
            var fimNovo = dataHora.AddMinutes(duracaoMinutos);

            var agendamentosDoBarbeiro = await context.Agendamentos
                .Include(a => a.Servico)
                .Where(a => a.BarbeiroId == barbeiroId
                    && a.Status != StatusAgendamento.Cancelado
                    && (ignorarAgendamentoId == null || a.Id != ignorarAgendamentoId))
                .AsNoTracking()
                .ToListAsync();

            var conflito = agendamentosDoBarbeiro.Any(a =>
            {
                var inicioExistente = a.DataHora;
                var fimExistente = a.DataHora.AddMinutes(a.Servico?.DuracaoMinutos ?? 30);
                return inicioNovo < fimExistente && fimNovo > inicioExistente;
            });

            if (conflito)
                throw new InvalidOperationException("Este barbeiro já possui um agendamento nesse horário.");
        }

        private Task<Agendamento?> BuscarComIncludesAsync(Guid id) =>
            context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Barbeiro)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.Id == id);
    }
}
