using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ServicioRepository repo;

        public ServicioController(IConfiguration config)
        {
            repo = new ServicioRepository(config.GetConnectionString("DefaultConnection"));
        }
        public IActionResult ServiciosVue()
        {
            return View();
        }
        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                repo.Crear(servicio);
                TempData["Mensaje"] = "Servicio registrado correctamente ✅";
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        public IActionResult Editar(int id)
        {
            var servicio = repo.ObtenerPorId(id);
            return View(servicio);
        }

        [HttpPost]
        public IActionResult Editar(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                // repo.Modificar(servicio);
                TempData["Mensaje"] = "Servicio modificado correctamente ✏️";
                return RedirectToAction("Index");
            }
            return View(servicio);
        }

        public IActionResult Eliminar(int id)
        {
            var servicio = repo.ObtenerPorId(id);
            return View(servicio);
        }

        [HttpPost]
        public IActionResult EliminarConfirmado(int id)
        {
            repo.Eliminar(id);
            TempData["Mensaje"] = "Servicio eliminado correctamente 🗑️";
            return RedirectToAction("Index");
        }
    }
}