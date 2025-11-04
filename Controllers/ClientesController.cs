using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepository repo;

        public ClientesController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repo = new ClienteRepository(conn);
        }

        public IActionResult Index(string? busqueda)
        {
            var clientes = repo.ObtenerTodos();

            if (!string.IsNullOrEmpty(busqueda))
            {
                var filtro = busqueda.Trim().ToLower();
                clientes = clientes
                    .Where(c => c.Dni.ToLower().Contains(filtro) ||
                                c.Nombre.ToLower().Contains(filtro) ||
                                c.Apellido.ToLower().Contains(filtro) ||
                                c.Email.ToLower().Contains(filtro))
                    .ToList();
            }

            ViewBag.Busqueda = busqueda; // <-- enviamos el valor al View
            return View(clientes);
        }
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                int nuevoId = repo.Crear(cliente);
                return RedirectToAction("Crear", "Vehiculos", new { ClienteId = nuevoId,origen = "cliente" });
                //return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public IActionResult Editar(int id)
        {
            var cliente = repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                repo.Editar(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public IActionResult Eliminar(int id)
        {
            var cliente = repo.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            repo.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
