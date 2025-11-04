using MySql.Data.MySqlClient;
using TallerMecanico.Models;

namespace TallerMecanico.Repositories
{
    public class UsuarioRepository
    {
        private readonly string connectionString;

        public UsuarioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"SELECT u.*, r.Nombre AS RolNombre
                          FROM Usuario u
                          INNER JOIN Rol r ON u.RolId = r.Id";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString()!,
                    Apellido = reader["Apellido"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    RolId = Convert.ToInt32(reader["RolId"]),
                    Avatar = reader["Avatar"]?.ToString(),
                    Rol = new Rol
                    {
                        Id = Convert.ToInt32(reader["RolId"]),
                        Nombre = reader["RolNombre"].ToString()!
                    }
                });
            }

            return lista;
        }

        public Usuario? ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Usuario WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                usuario = new Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString()!,
                    Apellido = reader["Apellido"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    RolId = Convert.ToInt32(reader["RolId"]),
                    Avatar = reader["Avatar"]?.ToString()
                };
            }

            return usuario;
        }

        public void Crear(Usuario u)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"INSERT INTO Usuario (Nombre, Apellido, Email, Password, RolId, Avatar)
                          VALUES (@nombre, @apellido, @email, @password, @rolId, @avatar)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", u.Nombre);
            command.Parameters.AddWithValue("@apellido", u.Apellido);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@password", u.Password);
            command.Parameters.AddWithValue("@rolId", u.RolId);
            command.Parameters.AddWithValue("@avatar", u.Avatar);
            command.ExecuteNonQuery();
        }

        public void Editar(Usuario u)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Usuario
                          SET Nombre=@nombre, Apellido=@apellido, Email=@email, 
                              RolId=@rolId, Avatar=@avatar
                          WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", u.Nombre);
            command.Parameters.AddWithValue("@apellido", u.Apellido);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@rolId", u.RolId);
            command.Parameters.AddWithValue("@avatar", u.Avatar);
            command.Parameters.AddWithValue("@id", u.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Usuario WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Usuario WHERE Email=@email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                usuario = new Usuario
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString()!,
                    Apellido = reader["Apellido"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Password = reader["Password"].ToString()!,
                    RolId = Convert.ToInt32(reader["RolId"]),
                    Avatar = reader["Avatar"]?.ToString()
                };
            }

            return usuario;
        }

        
    }
}
