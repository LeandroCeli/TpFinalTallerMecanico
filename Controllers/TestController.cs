using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Repositories;
using MySql.Data.MySqlClient;

namespace TallerMecanico.Controllers
{
    public class TestController : Controller
    {
        private readonly DbConnection _db;

        public TestController(DbConnection db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                using (var conn = _db.GetConnection())
                {
                    conn.Open();
                    ViewBag.Message = "✅ Conexión exitosa con MySQL";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "❌ Error: " + ex.Message;
            }

            return View();
        }
    }
}
