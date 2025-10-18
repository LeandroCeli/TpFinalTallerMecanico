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
            var query = "SELECT * FROM Clientes";
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
            var query = "SELECT * FROM Clientes WHERE Id = @id";
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

        public void Crear(Cliente cliente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"INSERT INTO Clientes (Dni, Nombre, Apellido, Telefono, Email, Direccion)
                          VALUES (@dni, @nombre, @apellido, @telefono, @email, @direccion)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@dni", cliente.Dni);
            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
            command.Parameters.AddWithValue("@apellido", cliente.Apellido);
            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
            command.Parameters.AddWithValue("@email", cliente.Email);
            command.Parameters.AddWithValue("@direccion", cliente.Direccion);
            command.ExecuteNonQuery();
        }

        public void Editar(Cliente cliente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Clientes SET Dni=@dni, Nombre=@nombre, Apellido=@apellido, 
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
            var query = "DELETE FROM Clientes WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
