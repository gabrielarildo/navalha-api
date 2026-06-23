using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.DTOs.UsuarioDto;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController(UsuarioService usuarioService) : ControllerBase
    {
        // Leia os verbos HTTP
        // Só nisso você já deve saber o que o endpoint faz
        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponseDto>>> Get() =>
            Ok(await usuarioService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(Guid id)
        {
            var usuario = await usuarioService.GetByIdAsync(id);
            return usuario is not null ? Ok(usuario) : NotFound();
        }
    }
}