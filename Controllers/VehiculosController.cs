using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly VehiculoRepository repoVehiculo;
        private readonly ClienteRepository repoCliente;

        public VehiculosController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoVehiculo = new VehiculoRepository(conn);
            repoCliente = new ClienteRepository(conn);
        }

        public IActionResult Index()
        {
            var lista = repoVehiculo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Crear()
        {
            ViewBag.Clientes = repoCliente.ObtenerTodos();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Vehiculo v)
        {
            if (ModelState.IsValid)
            {
                repoVehiculo.Crear(v);
                return RedirectToAction("Index");
            }
            ViewBag.Clientes = repoCliente.ObtenerTodos();
            return View(v);
        }

        public IActionResult Editar(int id)
        {
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            if (vehiculo == null) return NotFound();
            ViewBag.Clientes = repoCliente.ObtenerTodos();
            return View(vehiculo);
        }

        [HttpPost]
        public IActionResult Editar(Vehiculo v)
        {
            if (ModelState.IsValid)
            {
                repoVehiculo.Editar(v);
                return RedirectToAction("Index");
            }
            ViewBag.Clientes = repoCliente.ObtenerTodos();
            return View(v);
        }

        public IActionResult Eliminar(int id)
        {
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            if (vehiculo == null) return NotFound();
            return View(vehiculo);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            repoVehiculo.Eliminar(id);
            return RedirectToAction("Index");
        }

        public IActionResult DEtalles(int id)
        {
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            if (vehiculo == null) return NotFound();

            // Trae también el cliente si querés mostrarlo
            var cliente = repoCliente.ObtenerPorId(vehiculo.ClienteId);
            vehiculo.Cliente = cliente;

            return View(vehiculo);
        }




    }
}
