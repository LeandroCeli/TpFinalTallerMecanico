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

        public IActionResult Index(int page = 1, string? busqueda = null)
        {
            int pageSize = 5; // cantidad por página
            int totalRegistros;

            var clientes = repo.ObtenerPaginado(page, pageSize, out totalRegistros, busqueda);

            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);
            ViewBag.PaginaActual = page;
            ViewBag.Busqueda = busqueda;

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
                return RedirectToAction("Crear", "Vehiculos", new { ClienteId = nuevoId, origen = "cliente" });
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
