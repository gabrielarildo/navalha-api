using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.DTOs.ClienteDto;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController(ClienteService clienteService) : ControllerBase
    {
        /// <summary>Lista todos os clientes da barbearia Navalha.</summary>
        [HttpGet]
        public async Task<ActionResult<List<ClienteResponseDto>>> Get() =>
            Ok(await clienteService.GetAllAsync());

        /// <summary>Busca um cliente pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> GetById(Guid id)
        {
            var cliente = await clienteService.GetByIdAsync(id);
            return cliente is not null ? Ok(cliente) : NotFound();
        }

        /// <summary>Retorna o histórico de atendimentos (agendamentos) do cliente.</summary>
        [HttpGet("{id}/historico")]
        public async Task<ActionResult> GetHistorico(Guid id)
        {
            var cliente = await clienteService.GetByIdAsync(id);
            if (cliente is null) return NotFound();

            return Ok(await clienteService.GetHistoricoAsync(id));
        }

        /// <summary>Cadastra um novo cliente.</summary>
        [HttpPost]
        public async Task<ActionResult<ClienteResponseDto>> Post(ClienteCreateDto dto)
        {
            var cliente = await clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        /// <summary>Atualiza os dados de um cliente.</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> Put(Guid id, ClienteUpdateDto dto)
        {
            var cliente = await clienteService.UpdateAsync(id, dto);
            return cliente is not null ? Ok(cliente) : NotFound();
        }

        /// <summary>Remove um cliente.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) =>
            await clienteService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
