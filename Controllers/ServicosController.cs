using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.DTOs.ServicoDto;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicosController(ServicoService servicoService) : ControllerBase
    {
        /// <summary>Lista todos os serviços oferecidos pela Navalha.</summary>
        [HttpGet]
        public async Task<ActionResult<List<ServicoResponseDto>>> Get() =>
            Ok(await servicoService.GetAllAsync());

        /// <summary>Busca um serviço pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicoResponseDto>> GetById(Guid id)
        {
            var servico = await servicoService.GetByIdAsync(id);
            return servico is not null ? Ok(servico) : NotFound();
        }

        /// <summary>Cadastra um novo serviço.</summary>
        [HttpPost]
        public async Task<ActionResult<ServicoResponseDto>> Post(ServicoCreateDto dto)
        {
            var servico = await servicoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = servico.Id }, servico);
        }

        /// <summary>Atualiza um serviço existente.</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServicoResponseDto>> Put(Guid id, ServicoUpdateDto dto)
        {
            var servico = await servicoService.UpdateAsync(id, dto);
            return servico is not null ? Ok(servico) : NotFound();
        }

        /// <summary>Remove um serviço.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) =>
            await servicoService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
