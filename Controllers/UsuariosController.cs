using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Models;
using TallerMecanico.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace TallerMecanico.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository repoUsuario;
        private readonly RolRepository repoRol;

        // Límite de tamaño del avatar (5 MB)
        private const long MaxAvatarSize = 5 * 1024 * 1024;

        public UsuariosController(IConfiguration config)
        {
            string conn = config.GetConnectionString("DefaultConnection");
            repoUsuario = new UsuarioRepository(conn);
            repoRol = new RolRepository(conn);
        }

        // ───────────────────────────────────────────────
        // LISTADO
        // ───────────────────────────────────────────────
        public IActionResult Index()
        {
            var lista = repoUsuario.ObtenerTodos();
            return View(lista);
        }

        // ───────────────────────────────────────────────
        // CREAR
        // ───────────────────────────────────────────────
        public IActionResult Crear()
        {
            ViewBag.Roles = repoRol.ObtenerTodos();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Usuario u, IFormFile? avatar)
        {
            // 🔹 Validación de archivo
    if (avatar != null && avatar.Length > 0)
    {
        if (!avatar.ContentType.StartsWith("image/"))
        {
            ModelState.AddModelError("", "Solo se permiten archivos de imagen (jpg, png, etc.).");
        }
        else if (avatar.Length > MaxAvatarSize)
        {
            ModelState.AddModelError("", "El archivo es demasiado grande. Tamaño máximo: 5 MB.");
        }
        else
        {
            // Carpeta de destino
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
            Directory.CreateDirectory(ruta);

            // Nombre único
            string nombreArchivo = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
            string filePath = Path.Combine(ruta, nombreArchivo);

            using var stream = new FileStream(filePath, FileMode.Create);
            avatar.CopyTo(stream);

            u.Avatar = "/avatars/" + nombreArchivo;
        }
    }

    // 🔹 Generar hash de contraseña ANTES de guardar
    if (!string.IsNullOrEmpty(u.Password))
    {
        u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);
    }
    else
    {
        ModelState.AddModelError("PasswordHash", "Debe ingresar una contraseña.");
    }

    if (ModelState.IsValid)
    {
        repoUsuario.Crear(u);
        return RedirectToAction("Index");
    }

    ViewBag.Roles = repoRol.ObtenerTodos();
    return View(u);
        }

        // ───────────────────────────────────────────────
        // EDITAR
        // ───────────────────────────────────────────────
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
                if (!avatar.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("", "Solo se permiten archivos de imagen.");
                }
                else if (avatar.Length > MaxAvatarSize)
                {
                    ModelState.AddModelError("", "El archivo es demasiado grande. Tamaño máximo: 5 MB.");
                }
                else
                {
                    // Carpeta destino
                    string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                    Directory.CreateDirectory(ruta);

                    string nombreArchivo = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                    string filePath = Path.Combine(ruta, nombreArchivo);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    avatar.CopyTo(stream);

                    // Elimina avatar anterior si existe
                    if (!string.IsNullOrEmpty(u.Avatar))
                    {
                        var anterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", u.Avatar.TrimStart('/'));
                        if (System.IO.File.Exists(anterior))
                            System.IO.File.Delete(anterior);
                    }

                    u.Avatar = "/avatars/" + nombreArchivo;
                }
            }

            if (ModelState.IsValid)
            {
                repoUsuario.Editar(u);
                return RedirectToAction("Index");
            }

            ViewBag.Roles = repoRol.ObtenerTodos();
            return View(u);
        }

        // ───────────────────────────────────────────────
        // ELIMINAR
        // ───────────────────────────────────────────────
        public IActionResult Eliminar(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);

            // Eliminar avatar del servidor
            if (usuario != null && !string.IsNullOrEmpty(usuario.Avatar))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.Avatar.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            repoUsuario.Eliminar(id);
            return RedirectToAction("Index");
        }

        // ───────────────────────────────────────────────
        // Detalle
        // ───────────────────────────────────────────────
        public IActionResult Detalle(int id)
        {
            var usuario = repoUsuario.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            usuario.Rol = repoRol.ObtenerPorId(usuario.RolId);
            return View(usuario);
        }

        // 🔐 Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = repoUsuario.ObtenerPorEmail(email);
            if (usuario is null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View();
            }

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, usuario.Email ?? ""),
    new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}".Trim()),
    new Claim(ClaimTypes.Role, usuario.Rol?.ToString() ?? "Usuario") // Valor por defecto si Rol es null
};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        // 🔓 Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // 🚫 Acceso denegado
        [HttpGet]
        public IActionResult AccesoDenegado() => View();

        // 👤 Perfil del usuario logueado
        [Authorize]
        public IActionResult Perfil()
        {
            var email = User.Identity?.Name;
            var usuario = repoUsuario.ObtenerPorEmail(email ?? "");
            return View(usuario);
        }

        /*
               // 🔑 Cambiar contraseña
               [HttpPost]
               [Authorize]


               public IActionResult CambiarPassword(string actual, string nueva)
               {
                   var email = User.Identity?.Name;
                   var usuario = repoUsuario.ObtenerPorEmail(email ?? "");

                   if (!BCrypt.Net.BCrypt.Verify(actual, usuario.Password))
                   {
                       ModelState.AddModelError("", "Contraseña actual incorrecta");
                       return View("Perfil", usuario);
                   }

                   var nuevoHash = BCrypt.Net.BCrypt.HashPassword(nueva);
                   repoUsuario.ActualizarPassword(usuario.Id, nuevoHash);
                   return RedirectToAction("Perfil");
               }
               */

    }
}
