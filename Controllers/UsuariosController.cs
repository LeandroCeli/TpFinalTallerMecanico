using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;

namespace TallerMecanico.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository repoUsuario;
        private readonly RolRepository repoRol;

        public UsuariosController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoUsuario = new UsuarioRepository(conn);
            repoRol = new RolRepository(conn);
        }

        public IActionResult Index()
        {
            var lista = repoUsuario.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Crear()
        {
            ViewBag.Roles = repoRol.ObtenerTodos();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Usuario u, IFormFile? avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                string ruta = Path.Combine("wwwroot", "avatars");
                Directory.CreateDirectory(ruta);
                string filePath = Path.Combine(ruta, avatar.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                avatar.CopyTo(stream);

                u.Avatar = "/avatars/" + avatar.FileName;
            }

            if (ModelState.IsValid)
            {
                repoUsuario.Crear(u);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = repoRol.ObtenerTodos();
            return View(u);
        }

        public IActionResult Editar(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            ViewBag.Roles = repoRol.ObtenerTodos();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(Usuario u, IFormFile? avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                string ruta = Path.Combine("wwwroot", "avatars");
                Directory.CreateDirectory(ruta);
                string filePath = Path.Combine(ruta, avatar.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                avatar.CopyTo(stream);

                u.Avatar = "/avatars/" + avatar.FileName;
            }

            repoUsuario.Editar(u);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            repoUsuario.Eliminar(id);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            usuario.Rol = repoRol.ObtenerPorId(usuario.RolId);
            return View(usuario);
        }
    }
}
