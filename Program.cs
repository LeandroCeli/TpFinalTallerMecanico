using TallerMecanico.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// 1️⃣ Servicios de la aplicación
// -----------------------------
builder.Services.AddControllersWithViews();

// Servicio de acceso al contexto HTTP (para autenticación y vistas)
builder.Services.AddHttpContextAccessor();

// Servicio singleton para la conexión a BD
builder.Services.AddSingleton<TallerMecanico.Repositories.DbConnection>();


// -----------------------------
// 🔐 Autenticación por cookies
// -----------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";       // Página de login
        options.LogoutPath = "/Usuarios/Logout";     // Página de logout (opcional)
        options.AccessDeniedPath = "/Usuarios/AccesoDenegado"; // Opcional
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Duración de la cookie
        options.SlidingExpiration = true;           // Renovar cookie automáticamente
    });

var app = builder.Build();

// -----------------------------
// 2️⃣ Configuración del pipeline HTTP
// -----------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Seguridad HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔑 Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// -----------------------------
// 3️⃣ Rutas por defecto
// -----------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// -----------------------------
// 4️⃣ Ejecutar aplicación
// -----------------------------
app.Run();
