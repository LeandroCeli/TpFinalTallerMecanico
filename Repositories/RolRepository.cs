using MySql.Data.MySqlClient;
using TallerMecanico.Models;

namespace TallerMecanico.Repositories
{
    public class RolRepository
    {
        private readonly string connectionString;

        public RolRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Rol> ObtenerTodos()
        {
            var lista = new List<Rol>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT Id, Nombre, Descripcion FROM Rol";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Rol
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString()!,
                    Descripcion = reader["Descripcion"].ToString()!
                });
            }

            return lista;
        }

        public Rol? ObtenerPorId(int id)
        {
            Rol? rol = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT Id, Nombre, Descripcion FROM Rol WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                rol = new Rol
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString()!,
                    Descripcion = reader["Descripcion"].ToString()!
                };
            }

            return rol;
        }

        public void Crear(Rol r)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "INSERT INTO Roles (Nombre, Descripcion) VALUES (@nombre, @descripcion)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", r.Nombre);
            command.Parameters.AddWithValue("@descripcion", r.Descripcion);
            command.ExecuteNonQuery();
        }

        public void Editar(Rol r)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "UPDATE Roles SET Nombre=@nombre, Descripcion=@descripcion WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", r.Nombre);
            command.Parameters.AddWithValue("@descripcion", r.Descripcion);
            command.Parameters.AddWithValue("@id", r.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Roles WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
