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
                          FROM Vehiculo v
                          INNER JOIN Cliente c ON v.ClienteId = c.Id";
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
                    Anio = Convert.ToInt32(reader["Anio"]),
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
/*
        public Vehiculo? ObtenerPorId(int id)
        {
            Vehiculo? v = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Vehiculo WHERE Id=@id";
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
                    Anio = Convert.ToInt32(reader["Anio"]),
                    Color = reader["Color"].ToString()
                };
            }
            return v;
        }
*/
        // 🔹 NUEVO MÉTODO: Obtener vehículos por idCliente
        public List<Vehiculo> ObtenerPorCliente(int idCliente)
        {
            var lista = new List<Vehiculo>();
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"SELECT v.*, c.Nombre, c.Apellido 
                          FROM Vehiculo v
                          INNER JOIN Cliente c ON v.ClienteId = c.Id
                          WHERE v.ClienteId = @idCliente";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@idCliente", idCliente);
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
                    Anio = Convert.ToInt32(reader["Anio"]),
                    Color = reader["Color"].ToString(),
                    Kilometraje = Convert.ToInt32(reader["Kilometraje"]),
                    Cliente = new Cliente
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString()
                    }
                });
            }
            return lista;
        }


        public Vehiculo ObtenerPorId(int id)
        {
            Vehiculo? vehiculo = null;
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var sql = @"SELECT v.*, c.Id AS ClienteId, c.Nombre, c.Apellido, c.Dni, c.Telefono
                FROM Vehiculo v
                INNER JOIN Cliente c ON v.ClienteId = c.Id
                WHERE v.Id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                vehiculo = new Vehiculo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ClienteId = Convert.ToInt32(reader["ClienteId"]),
                    Patente = reader["Patente"].ToString(),
                    Marca = reader["Marca"].ToString(),
                    Modelo = reader["Modelo"].ToString(),
                    Anio = Convert.ToInt32(reader["Anio"]),
                    Color = reader["Color"].ToString(),
                    Kilometraje = Convert.ToInt32(reader["Kilometraje"]),
                    Tipo = reader["Tipo"].ToString(),
                    Cliente = new Cliente
                    {
                        Id = Convert.ToInt32(reader["ClienteId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Dni = reader["Dni"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    }
                };
            }

            return vehiculo;
        }


        public void Crear(Vehiculo v)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            // ✅ Validación de patente existente
            if (PatenteExiste(v.Patente))
                throw new Exception("La patente ingresada ya existe en el sistema.");

            var query = @"
        INSERT INTO Vehiculo 
        (ClienteId, Patente, Marca, Modelo, Anio, Color, Kilometraje, Tipo, FechaRegistro, Activo)
        VALUES 
        (@clienteId, @patente, @marca, @modelo, @anio, @color, @kilometraje, @tipo, NOW(), 1);
    ";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteId", v.ClienteId);
            command.Parameters.AddWithValue("@patente", v.Patente);
            command.Parameters.AddWithValue("@marca", v.Marca ?? "");
            command.Parameters.AddWithValue("@modelo", v.Modelo ?? "");
            command.Parameters.AddWithValue("@anio", v.Anio);
            command.Parameters.AddWithValue("@color", v.Color ?? "");
            command.Parameters.AddWithValue("@kilometraje", v.Kilometraje);
            command.Parameters.AddWithValue("@tipo", v.Tipo ?? "");

            command.ExecuteNonQuery();
        }



        public void Editar(Vehiculo v)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = @"UPDATE Vehiculo 
                          SET ClienteId=@clienteId, Patente=@patente, Marca=@marca, 
                              Modelo=@modelo, Anio=@Anio, Color=@color
                          WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@clienteId", v.ClienteId);
            command.Parameters.AddWithValue("@patente", v.Patente);
            command.Parameters.AddWithValue("@marca", v.Marca);
            command.Parameters.AddWithValue("@modelo", v.Modelo);
            command.Parameters.AddWithValue("@Anio", v.Anio);
            command.Parameters.AddWithValue("@color", v.Color);
            command.Parameters.AddWithValue("@id", v.Id);
            command.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            var query = "DELETE FROM Vehiculo WHERE Id=@id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public List<Vehiculo> GetAll()
        {
            return ObtenerTodos();
        }

        public Vehiculo? ObtenerPorPatente(string patente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = @"SELECT v.*, c.Nombre AS ClienteNombre, c.Telefono AS ClienteTelefono
                  FROM Vehiculo v
                  INNER JOIN Cliente c ON c.Id = v.ClienteId
                  WHERE v.Patente = @patente";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@patente", patente);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Vehiculo
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    ClienteId = reader["ClienteId"] != DBNull.Value ? Convert.ToInt32(reader["ClienteId"]) : 0,
                    Patente = reader["Patente"]?.ToString() ?? "",
                    Marca = reader["Marca"]?.ToString() ?? "",
                    Modelo = reader["Modelo"]?.ToString() ?? "",
                    Anio = reader["Anio"] != DBNull.Value ? Convert.ToInt32(reader["Anio"]) : 0,
                    Color = reader["Color"]?.ToString() ?? "",
                    Kilometraje = reader["Kilometraje"] != DBNull.Value ? Convert.ToInt32(reader["Kilometraje"]) : 0,
                    Tipo = reader["Tipo"]?.ToString() ?? "",
                    Cliente = new Cliente
                    {
                        Id = reader["ClienteId"] != DBNull.Value ? Convert.ToInt32(reader["ClienteId"]) : 0,
                        Nombre = reader["ClienteNombre"]?.ToString() ?? "",
                        Telefono = reader["ClienteTelefono"]?.ToString() ?? ""
                    }
                };
            }

            return null;
        }
        public void ActualizarKilometraje(int idVehiculo, int nuevoKilometraje)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = @"UPDATE Vehiculo 
                  SET Kilometraje = @km 
                  WHERE Id = @id";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@km", nuevoKilometraje);
            command.Parameters.AddWithValue("@id", idVehiculo);

            command.ExecuteNonQuery();
        }


        public bool PatenteExiste(string patente)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = "SELECT COUNT(*) FROM Vehiculo WHERE Patente = @patente";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@patente", patente);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }

    }
}
