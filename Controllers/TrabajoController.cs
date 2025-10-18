using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class TrabajoController : Controller
    {
        private readonly TrabajoRepository repo;
        private readonly VehiculoRepository repoVehiculo;

        public TrabajoController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            repo = new TrabajoRepository(connectionString);
            repoVehiculo = new VehiculoRepository(connectionString);
        }

        public IActionResult Index()
        {
            var lista = repo.GetAll();
            return View(lista);
        }

        public IActionResult Detalles(int id)
        {
            var trabajo = repo.GetById(id);
            if (trabajo == null) return NotFound();
            return View(trabajo);
        }

        public IActionResult Crear()
        {
            ViewBag.Vehiculos = repoVehiculo.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Trabajo trabajo)
        {
            if (ModelState.IsValid)
            {
                repo.Create(trabajo);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Vehiculos = repoVehiculo.GetAll();
            return View(trabajo);
        }

        public IActionResult Editar(int id)
        {
            var trabajo = repo.GetById(id);
            if (trabajo == null) return NotFound();
            ViewBag.Vehiculos = repoVehiculo.GetAll();
            return View(trabajo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Trabajo trabajo)
        {
            if (ModelState.IsValid)
            {
                repo.Update(trabajo);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Vehiculos = repoVehiculo.GetAll();
            return View(trabajo);
        }

        public IActionResult Eliminar(int id)
        {
            var trabajo = repo.GetById(id);
            if (trabajo == null) return NotFound();
            return View(trabajo);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
