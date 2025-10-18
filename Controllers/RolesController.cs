using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class RolesController : Controller
    {
        private readonly RolRepository repoRol;

        public RolesController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoRol = new RolRepository(conn);
        }

        public IActionResult Index()
        {
            var lista = repoRol.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Rol r)
        {
            if (ModelState.IsValid)
            {
                repoRol.Crear(r);
                return RedirectToAction("Index");
            }
            return View(r);
        }

        public IActionResult Editar(int id)
        {
            var rol = repoRol.ObtenerPorId(id);
            if (rol == null) return NotFound();
            return View(rol);
        }

        [HttpPost]
        public IActionResult Editar(Rol r)
        {
            if (ModelState.IsValid)
            {
                repoRol.Editar(r);
                return RedirectToAction("Index");
            }
            return View(r);
        }

        public IActionResult Eliminar(int id)
        {
            var rol = repoRol.ObtenerPorId(id);
            if (rol == null) return NotFound();
            return View(rol);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            repoRol.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
