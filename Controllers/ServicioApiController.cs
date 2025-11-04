using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioApiController : ControllerBase
    {
        private readonly ServicioRepository repo;

        public ServicioApiController(IConfiguration config)
        {
            repo = new ServicioRepository(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var servicios = repo.ObtenerTodos();
            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var servicio = repo.ObtenerPorId(id);
            if (servicio == null) return NotFound();
            return Ok(servicio);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Servicio servicio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nuevoId = repo.Crear(servicio);
            servicio.Id = nuevoId;
            return CreatedAtAction(nameof(Get), new { id = nuevoId }, servicio);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Servicio servicio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existente = repo.ObtenerPorId(id);
            if (existente == null) return NotFound();
            servicio.Id = id;
            repo.Editar(servicio);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existente = repo.ObtenerPorId(id);
            if (existente == null) return NotFound();
            repo.Eliminar(id);
            return NoContent();
        }
    }
}