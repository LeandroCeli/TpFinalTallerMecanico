using System.Data;
using MySql.Data.MySqlClient;
using TallerMecanico.Models;

namespace TallerMecanico.Repositories
{
    public class VehiculoRepository
    {
        private readonly string connectionString;

        public VehiculoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Vehiculo> ObtenerTodos()
        {
            var lista = new List<Vehiculo>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"SELECT v.*, c.Nombre, c.Apellido 
                          FROM Vehiculos v
                          INNER JOIN Clientes c ON v.ClienteId = c.Id";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Vehiculo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ClienteId = Convert.ToInt32(reader["ClienteId"]),
                    Patente = reader["Patente"].ToString(),
                    Marca = reader["Marca"].ToString(),
                    Modelo = reader["Modelo"].ToString(),
                    Año = Convert.ToInt32(reader["Año"]),
                    Color = reader["Color"].ToString(),
                    Cliente = new Cliente
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString()
                    }
                });
            }
            return lista;
        }

        public Vehiculo? ObtenerPorId(int id)
        {
            Vehiculo? v = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Vehiculos WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                v = new Vehiculo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ClienteId = Convert.ToInt32(reader["ClienteId"]),
                    Patente = reader["Patente"].ToString(),
                    Marca = reader["Marca"].ToString(),
                    Modelo = reader["Modelo"].ToString(),
                    Año = Convert.ToInt32(reader["Año"]),
                    Color = reader["Color"].ToString()
                };
            }
            return v;
        }

        public void Crear(Vehiculo v)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"INSERT INTO Vehiculos (ClienteId, Patente, Marca, Modelo, Año, Color)
                          VALUES (@clienteId, @patente, @marca, @modelo, @año, @color)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteId", v.ClienteId);
            command.Parameters.AddWithValue("@patente", v.Patente);
            command.Parameters.AddWithValue("@marca", v.Marca);
            command.Parameters.AddWithValue("@modelo", v.Modelo);
            command.Parameters.AddWithValue("@año", v.Año);
            command.Parameters.AddWithValue("@color", v.Color);
            command.ExecuteNonQuery();
        }

        public void Editar(Vehiculo v)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Vehiculos 
                          SET ClienteId=@clienteId, Patente=@patente, Marca=@marca, 
                              Modelo=@modelo, Año=@año, Color=@color
                          WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteId", v.ClienteId);
            command.Parameters.AddWithValue("@patente", v.Patente);
            command.Parameters.AddWithValue("@marca", v.Marca);
            command.Parameters.AddWithValue("@modelo", v.Modelo);
            command.Parameters.AddWithValue("@año", v.Año);
            command.Parameters.AddWithValue("@color", v.Color);
            command.Parameters.AddWithValue("@id", v.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Vehiculos WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
