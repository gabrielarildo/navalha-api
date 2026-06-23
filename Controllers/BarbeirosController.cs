using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.DTOs.BarbeiroDto;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarbeirosController(BarbeiroService barbeiroService) : ControllerBase
    {
        /// <summary>Lista todos os barbeiros da Navalha.</summary>
        [HttpGet]
        public async Task<ActionResult<List<BarbeiroResponseDto>>> Get() =>
            Ok(await barbeiroService.GetAllAsync());

        /// <summary>Busca um barbeiro pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BarbeiroResponseDto>> GetById(Guid id)
        {
            var barbeiro = await barbeiroService.GetByIdAsync(id);
            return barbeiro is not null ? Ok(barbeiro) : NotFound();
        }

        /// <summary>
        /// Retorna os horários disponíveis de um barbeiro em uma data específica.
        /// Exemplo: /api/barbeiros/{id}/horarios-disponiveis?data=2026-06-25
        /// </summary>
        [HttpGet("{id}/horarios-disponiveis")]
        public async Task<ActionResult> GetHorariosDisponiveis(Guid id, [FromQuery] DateOnly data)
        {
            var barbeiro = await barbeiroService.GetByIdAsync(id);
            if (barbeiro is null) return NotFound();

            var horarios = await barbeiroService.GetHorariosDisponiveisAsync(id, data);
            return Ok(horarios);
        }

        /// <summary>Cadastra um novo barbeiro.</summary>
        [HttpPost]
        public async Task<ActionResult<BarbeiroResponseDto>> Post(BarbeiroCreateDto dto)
        {
            var barbeiro = await barbeiroService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = barbeiro.Id }, barbeiro);
        }

        /// <summary>Atualiza os dados de um barbeiro.</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<BarbeiroResponseDto>> Put(Guid id, BarbeiroUpdateDto dto)
        {
            var barbeiro = await barbeiroService.UpdateAsync(id, dto);
            return barbeiro is not null ? Ok(barbeiro) : NotFound();
        }

        /// <summary>Remove um barbeiro.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) =>
            await barbeiroService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
