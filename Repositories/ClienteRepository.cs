using System.Data;
using MySql.Data.MySqlClient;
using TallerMecanico.Models;

namespace TallerMecanico.Repositories
{
    public class ClienteRepository
    {
        private readonly string connectionString;

        public ClienteRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Cliente> ObtenerTodos()
        {
            var lista = new List<Cliente>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Cliente";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Cliente
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Dni = reader["Dni"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Apellido = reader["Apellido"].ToString(),
                    Telefono = reader["Telefono"].ToString(),
                    Email = reader["Email"].ToString(),
                    Direccion = reader["Direccion"].ToString()
                });
            }
            return lista;
        }

        public Cliente? ObtenerPorId(int id)
        {
            Cliente? cliente = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Cliente WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                cliente = new Cliente
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Dni = reader["Dni"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Apellido = reader["Apellido"].ToString(),
                    Telefono = reader["Telefono"].ToString(),
                    Email = reader["Email"].ToString(),
                    Direccion = reader["Direccion"].ToString()
                };
            }
            return cliente;
        }

        public int Crear(Cliente cliente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = @"INSERT INTO Cliente (Dni, Nombre, Apellido, Telefono, Email, Direccion)
                  VALUES (@dni, @nombre, @apellido, @telefono, @email, @direccion)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@dni", cliente.Dni);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@apellido", cliente.Apellido);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@direccion", cliente.Direccion);

            command.ExecuteNonQuery();

            // 👇 Esta línea obtiene el último ID autogenerado
            int nuevoId = (int)command.LastInsertedId;

            return nuevoId;
        }


        public void Editar(Cliente cliente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Cliente SET Dni=@dni, Nombre=@nombre, Apellido=@apellido, 
                          Telefono=@telefono, Email=@email, Direccion=@direccion 
                          WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@dni", cliente.Dni);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@apellido", cliente.Apellido);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@direccion", cliente.Direccion);
            command.Parameters.AddWithValue("@id", cliente.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Cliente WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Cliente> ObtenerPaginado(int page, int pageSize, out int totalRegistros, string? filtro = null)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            string where = "";
            if (!string.IsNullOrEmpty(filtro))
                where = "WHERE Nombre LIKE @filtro OR Apellido LIKE @filtro OR Dni LIKE @filtro OR Email LIKE @filtro";

            string sqlCount = $"SELECT COUNT(*) FROM Cliente {where}";
            using var cmdCount = new MySqlCommand(sqlCount, connection);
            if (!string.IsNullOrEmpty(filtro))
                cmdCount.Parameters.AddWithValue("@filtro", $"%{filtro}%");

            totalRegistros = Convert.ToInt32(cmdCount.ExecuteScalar());

            int offset = (page - 1) * pageSize;

            string sql = $@"
                            SELECT * FROM Cliente
                            {where}
                            ORDER BY Apellido, Nombre
                            LIMIT @size OFFSET @offset";

            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@size", pageSize);
            cmd.Parameters.AddWithValue("@offset", offset);
            if (!string.IsNullOrEmpty(filtro))
                cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");

            using var reader = cmd.ExecuteReader();
            var clientes = new List<Cliente>();

            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Dni = reader.GetString("Dni"),
                    Telefono = reader.GetString("Telefono"),
                    Email = reader.GetString("Email")
                });
            }

            return clientes;
        }




    }
}
