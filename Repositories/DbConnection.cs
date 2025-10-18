using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace TallerMecanico.Repositories
{
    public class DbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
