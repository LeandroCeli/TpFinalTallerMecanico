using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ClienteRepository repoClientes;
        private readonly VehiculoRepository repoVehiculos;
        private readonly TrabajoRepository repoTrabajos;

        public ReportesController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoClientes = new ClienteRepository(conn);
            repoVehiculos = new VehiculoRepository(conn);
            repoTrabajos = new TrabajoRepository(conn);
        }

        public IActionResult Index()
        {
            // 📊 Datos de resumen
            var totalClientes = repoClientes.ObtenerTodos().Count;
            var totalVehiculos = repoVehiculos.ObtenerTodos().Count;
            var totalTrabajos = repoTrabajos.GetAll().Count;

            // 🧾 Últimos 5 trabajos realizados
            var ultimosTrabajos = repoTrabajos.GetAll()
                .OrderByDescending(t => t.FechaFin)
                .Take(5)
                .ToList();

            ViewBag.TotalClientes = totalClientes;
            ViewBag.TotalVehiculos = totalVehiculos;
            ViewBag.TotalTrabajos = totalTrabajos;
            ViewBag.UltimosTrabajos = ultimosTrabajos;

            return View();
        }
    }
}
