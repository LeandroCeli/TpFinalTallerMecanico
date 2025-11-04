using System.Data;
using MySql.Data.MySqlClient;
using TallerMecanico.Models;

namespace TallerMecanico.Repositories
{
    public class ServicioRepository
    {
        private readonly string connectionString;

        public ServicioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Servicio> ObtenerTodos()
        {
            var lista = new List<Servicio>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Servicio";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Servicio
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    CostoBase = Convert.ToDecimal(reader["CostoBase"]),
                    IncluyeInsumos = Convert.ToBoolean(reader["IncluyeInsumos"])
                });
            }
            return lista;
        }

        public Servicio? ObtenerPorId(int id)
        {
            Servicio? servicio = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Servicio WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                servicio = new Servicio
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    CostoBase = Convert.ToDecimal(reader["CostoBase"]),
                    IncluyeInsumos = Convert.ToBoolean(reader["IncluyeInsumos"])
                };
            }
            return servicio;
        }

        public int Crear(Servicio servicio)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"INSERT INTO Servicio (Nombre, Descripcion, CostoBase, IncluyeInsumos)
                          VALUES (@nombre, @descripcion, @costoBase, @incluyeInsumos)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", servicio.Nombre);
            command.Parameters.AddWithValue("@descripcion", servicio.Descripcion);
            command.Parameters.AddWithValue("@costoBase", servicio.CostoBase);
            command.Parameters.AddWithValue("@incluyeInsumos", servicio.IncluyeInsumos);
            command.ExecuteNonQuery();
            return (int)command.LastInsertedId;
        }

        public void Editar(Servicio servicio)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Servicio SET Nombre=@nombre, Descripcion=@descripcion,
                          CostoBase=@costoBase, IncluyeInsumos=@incluyeInsumos WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", servicio.Nombre);
            command.Parameters.AddWithValue("@descripcion", servicio.Descripcion);
            command.Parameters.AddWithValue("@costoBase", servicio.CostoBase);
            command.Parameters.AddWithValue("@incluyeInsumos", servicio.IncluyeInsumos);
            command.Parameters.AddWithValue("@id", servicio.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Servicio WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }




    }
}