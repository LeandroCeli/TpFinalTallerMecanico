using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajoApiController : ControllerBase
    {
        private readonly TrabajoRepository repoTrabajo;
        private readonly VehiculoRepository repoVehiculo;
        private readonly ServicioRepository repoServicio;

        public TrabajoApiController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            repoTrabajo = new TrabajoRepository(connectionString);
            repoVehiculo = new VehiculoRepository(connectionString);
            repoServicio = new ServicioRepository(connectionString);
        }

        // 🔍 Buscar vehículo por patente
        [HttpGet("BuscarVehiculo")]
        public IActionResult BuscarVehiculo(string patente)
        {
            var vehiculo = repoVehiculo.ObtenerPorPatente(patente);
            if (vehiculo == null) return NotFound();
            return Ok(vehiculo);
        }

        // 📋 Obtener servicios disponibles
        [HttpGet("Servicios")]
        public IActionResult ObtenerServicios()
        {
            var servicios = repoServicio.ObtenerTodos();
            return Ok(servicios);
        }

        // 💾 Guardar trabajo con servicios realizados
        [HttpPost("Guardar")]
        public IActionResult GuardarTrabajo([FromBody] Trabajo trabajo)
        {

            // trabajo.UsuarioId = ObtenerUsuarioId();
            var id = repoTrabajo.Create(trabajo);


            return Ok(new { id });
        }

        // 📜 Obtener historial por vehículo
        [HttpGet("Historial")]
        public IActionResult HistorialPorVehiculo(string patente)
        {
            var vehiculo = repoVehiculo.ObtenerPorPatente(patente);
            if (vehiculo == null) return NotFound();

            var historial = repoTrabajo.GetHistorialPorVehiculo(vehiculo.Id);
            return Ok(historial);
        }

    }
}