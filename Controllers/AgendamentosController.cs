using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.DTOs.AgendamentoDto;
using TodoApi.Models.Entities;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController(AgendamentoService agendamentoService) : ControllerBase
    {
        /// <summary>Lista todos os agendamentos da barbearia.</summary>
        [HttpGet]
        public async Task<ActionResult<List<AgendamentoResponseDto>>> Get() =>
            Ok(await agendamentoService.GetAllAsync());

        /// <summary>Busca um agendamento pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AgendamentoResponseDto>> GetById(Guid id)
        {
            var agendamento = await agendamentoService.GetByIdAsync(id);
            return agendamento is not null ? Ok(agendamento) : NotFound();
        }

        /// <summary>Lista os agendamentos de um barbeiro específico.</summary>
        [HttpGet("barbeiro/{barbeiroId}")]
        public async Task<ActionResult<List<AgendamentoResponseDto>>> GetByBarbeiro(Guid barbeiroId) =>
            Ok(await agendamentoService.GetByBarbeiroAsync(barbeiroId));

        /// <summary>
        /// Cria um novo agendamento. O valor é calculado automaticamente a partir do
        /// serviço escolhido. Não são permitidos agendamentos em datas passadas ou em
        /// horários já ocupados pelo barbeiro selecionado.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AgendamentoResponseDto>> Post(AgendamentoCreateDto dto)
        {
            try
            {
                var agendamento = await agendamentoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = agendamento.Id }, agendamento);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Reagenda um agendamento (altera serviço, data/horário e/ou observações).</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AgendamentoResponseDto>> Put(Guid id, AgendamentoUpdateDto dto)
        {
            try
            {
                var agendamento = await agendamentoService.UpdateAsync(id, dto);
                return agendamento is not null ? Ok(agendamento) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Atualiza o status do agendamento (Agendado, Confirmado, Concluído ou Cancelado).</summary>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<AgendamentoResponseDto>> PatchStatus(Guid id, AgendamentoStatusUpdateDto dto)
        {
            try
            {
                var agendamento = await agendamentoService.AtualizarStatusAsync(id, dto.Status);
                return agendamento is not null ? Ok(agendamento) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Cancela um agendamento. Só é permitido antes do horário agendado.</summary>
        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult<AgendamentoResponseDto>> Cancelar(Guid id)
        {
            try
            {
                var agendamento = await agendamentoService.CancelarAsync(id);
                return agendamento is not null ? Ok(agendamento) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
