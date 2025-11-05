using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly VehiculoRepository repoVehiculo;
        private readonly ClienteRepository repoCliente;
        private readonly TrabajoRepository repoTrabajo;

        public VehiculosController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoVehiculo = new VehiculoRepository(conn);
            repoCliente = new ClienteRepository(conn);
            repoTrabajo = new TrabajoRepository(conn);
        }

        // 🔹 Muestra todos los vehículos (vista general)
        public IActionResult Index(string? busqueda, int page = 1)
        {
            const int pageSize = 5; // cantidad de vehículos por página

            var vehiculos = repoVehiculo.ObtenerTodos();

            // 🔍 FILTRO
            if (!string.IsNullOrEmpty(busqueda))
            {
                var filtro = busqueda.Trim().ToLower();
                vehiculos = vehiculos
                    .Where(v =>
                        (v.Patente?.ToLower().Contains(filtro) ?? false) ||
                        (v.Marca?.ToLower().Contains(filtro) ?? false) ||
                        (v.Modelo?.ToLower().Contains(filtro) ?? false) ||
                        (v.Color?.ToLower().Contains(filtro) ?? false) ||
                        (v.Cliente?.Nombre?.ToLower().Contains(filtro) ?? false) ||
                        (v.Cliente?.Apellido?.ToLower().Contains(filtro) ?? false)
                    )
                    .ToList();
            }

            // 📊 PAGINADO
            int totalRegistros = vehiculos.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            vehiculos = vehiculos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 📨 ENVIAR DATOS A LA VISTA
            ViewBag.Busqueda = busqueda;
            ViewBag.PaginaActual = page;
            ViewBag.TotalPaginas = totalPaginas;

            return View(vehiculos);
        }


        // 🔹 NUEVO: Muestra vehículos filtrados por cliente

        /*  public IActionResult PorCliente(int idCliente)
          {
              // Obtener cliente para mostrar su nombre en la vista
              var cliente = repoCliente.ObtenerPorId(idCliente);
              if (cliente == null)
              {
                  return NotFound();
              }

              // Obtener vehículos asociados a ese cliente
              var vehiculos = repoVehiculo.ObtenerPorCliente(idCliente);

              // Pasar cliente a la vista
              ViewBag.Cliente = cliente;

              return View("Index", vehiculos); // Reutiliza la misma vista Index
          }
  */
        public IActionResult PorCliente(int idCliente)
        {
            var cliente = repoCliente.ObtenerPorId(idCliente);
            var vehiculos = repoVehiculo.ObtenerPorCliente(idCliente);

            ViewBag.Cliente = cliente;
            return View("PorCliente", vehiculos);
        }



        // ------------------------- CRUD -------------------------

        public IActionResult Crear(int? ClienteId, String origen)
        {
            var vehiculo = new Vehiculo { ClienteId = ClienteId ?? 0 };
            ViewData["Origen"] = origen;
            return View(vehiculo);
        }

        [HttpPost]
        public IActionResult Crear(Vehiculo v, string origen)
        {
            ModelState.Remove("origen");
            if (ModelState.IsValid)
            {
                try
                {
                    repoVehiculo.Crear(v);
                    TempData["Mensaje"] = "Vehículo registrado correctamente ✅";

                    // ✅ Si vino desde Clientes, volvemos a la vista de vehículos del cliente
                    if (origen == "cliente")
                    {
                        return RedirectToAction("PorCliente", "Vehiculos", new { idCliente = v.ClienteId });
                    }

                    // ✅ Si vino desde menú Vehículos, volvemos a Vehículos/Index
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(v);
                }
            }

            // ❌ Si el modelo no es válido
            ViewBag.Clientes = repoCliente.ObtenerTodos();
            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    Console.WriteLine($"❌ Campo: {error.Key} - Error: {subError.ErrorMessage}");
                }
            }
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
                return RedirectToAction("PorCliente", new { idCliente = v.ClienteId });
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
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            repoVehiculo.Eliminar(id);
            return RedirectToAction("PorCliente", new { idCliente = vehiculo.ClienteId });
        }

        public IActionResult Detalles(int id)
        {
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            if (vehiculo == null) return NotFound();

            var cliente = repoCliente.ObtenerPorId(vehiculo.ClienteId);
            vehiculo.Cliente = cliente;

            return View(vehiculo);
        }
        public IActionResult Historial(int id)
        {
            var vehiculo = repoVehiculo.ObtenerPorId(id);
            if (vehiculo == null) return NotFound();

            // ✅ Si Cliente es null, lo buscamos con ClienteId
            if (vehiculo.Cliente == null && vehiculo.ClienteId > 0)
            {
                vehiculo.Cliente = repoCliente.ObtenerPorId(vehiculo.ClienteId);
            }


            var historial = repoTrabajo.GetHistorialPorVehiculo(id);

            ViewBag.Vehiculo = vehiculo;
            return View(historial);
        }

    }
}
