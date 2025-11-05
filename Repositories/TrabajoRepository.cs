using MySql.Data.MySqlClient;
using TallerMecanico.Models;
using System.Collections.Generic;

namespace TallerMecanico.Repositories
{
    public class TrabajoRepository
    {
        private readonly string connectionString;

        public TrabajoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Trabajo> GetAll()
        {
            var lista = new List<Trabajo>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"SELECT t.*, v.Patente, v.Marca, v.Modelo
                            FROM trabajos t
                            INNER JOIN vehiculos v ON v.id_vehiculo = t.id_vehiculo
                            ORDER BY t.fecha_inicio DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Trabajo
                        {
                            Id = reader.GetInt32("id_trabajo"),
                            IdVehiculo = reader.GetInt32("id_vehiculo"),
                            Observaciones = reader.GetString("Observaciones"),

                            FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime("fecha_fin"),
                            CostoTotal = reader.GetDecimal("costo_total"),
                            Estado = reader.GetString("estado"),
                            Vehiculo = new Vehiculo
                            {
                                Id = reader.GetInt32("id_vehiculo"),
                                Patente = reader.GetString("Patente"),
                                Marca = reader.GetString("Marca"),
                                Modelo = reader.GetString("Modelo")
                            }
                        });
                    }
                }
            }
            return lista;
        }

        public Trabajo? GetById(int id)
        {
            Trabajo? trabajo = null;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = "SELECT * FROM trabajos WHERE id_trabajo=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            trabajo = new Trabajo
                            {
                                Id = reader.GetInt32("id_trabajo"),
                                IdVehiculo = reader.GetInt32("id_vehiculo"),
                                Observaciones = reader.GetString("Observaciones"),

                                FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime("fecha_fin"),
                                CostoTotal = reader.GetDecimal("costo_total"),
                                Estado = reader.GetString("estado")
                            };
                        }
                    }
                }
            }
            return trabajo;
        }

        public int Create(Trabajo trabajo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // 1. Insertar el trabajo
                var sqlTrabajo = @"
                        INSERT INTO Trabajo 
                        (VehiculoId, UsuarioId, Observaciones, FechaEntrega, Estado, KilometrajeSalida)
                        VALUES 
                        (@vehiculo, @usuario, @obs, @fin, @estado, @kmsalida);
                        SELECT LAST_INSERT_ID();";

                int idTrabajo;

                using (var cmd = new MySqlCommand(sqlTrabajo, conn))
                {
                    cmd.Parameters.AddWithValue("@vehiculo", trabajo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@usuario", trabajo.UsuarioId);
                    cmd.Parameters.AddWithValue("@obs", trabajo.Observaciones ?? "");
                    cmd.Parameters.AddWithValue("@fin", (object?)trabajo.FechaFin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", trabajo.Estado);
                    cmd.Parameters.AddWithValue("@kmsalida", trabajo.KilometrajeSalida);

                    idTrabajo = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Insertar los servicios realizados
                foreach (var servicio in trabajo.ServiciosRealizados)
                {
                    var sqlServicio = @"
                INSERT INTO TrabajoServicio 
                (TrabajoId, ServicioId, CostoAplicado)
                VALUES 
                (@trabajoId, @servicioId, @costo);";

                    using (var cmd = new MySqlCommand(sqlServicio, conn))
                    {
                        cmd.Parameters.AddWithValue("@trabajoId", idTrabajo);
                        cmd.Parameters.AddWithValue("@servicioId", servicio.ServicioId);
                        cmd.Parameters.AddWithValue("@costo", servicio.CostoAplicado);

                        cmd.ExecuteNonQuery();
                    }
                }

                return idTrabajo;
            }
        }

        public void Update(Trabajo trabajo)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"UPDATE trabajos SET
                            id_vehiculo=@vehiculo,
                            descripcion=@desc,
                            fecha_fin=@fin,
                            costo_total=@costo,
                            estado=@estado
                            WHERE id_trabajo=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@vehiculo", trabajo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@desc", trabajo.Observaciones);
                    cmd.Parameters.AddWithValue("@fin", (object?)trabajo.FechaFin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", trabajo.CostoTotal);
                    cmd.Parameters.AddWithValue("@estado", trabajo.Estado);
                    cmd.Parameters.AddWithValue("@id", trabajo.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = "DELETE FROM trabajos WHERE id_trabajo=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Trabajo> GetHistorialPorVehiculo(int idVehiculo)
        {
            var lista = new List<Trabajo>();

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            var query = @"
                        SELECT 
                            t.Id AS TrabajoId, t.Observaciones, t.FechaEntrega, t.Estado,
                            t.KilometrajeSalida, -- ✅ agregado
                            v.Id AS VehiculoId, v.Patente, v.Marca, v.Modelo,
                            ts.ServicioId, s.Nombre AS ServicioNombre, ts.CostoAplicado, ts.Observaciones
                        FROM Trabajo t
                        INNER JOIN Vehiculo v ON v.Id = t.VehiculoId
                        INNER JOIN TrabajoServicio ts ON ts.TrabajoId = t.Id
                        INNER JOIN Servicio s ON s.Id = ts.ServicioId
                        WHERE t.VehiculoId = @id
                        ORDER BY t.FechaEntrega DESC;
                    ";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", idVehiculo);

            using var reader = command.ExecuteReader();

            Trabajo trabajoActual = null;

            while (reader.Read())
            {
                int trabajoId = Convert.ToInt32(reader["TrabajoId"]);

                // ✅ Si cambia el trabajo, crear nuevo
                if (trabajoActual == null || trabajoActual.Id != trabajoId)
                {
                    trabajoActual = new Trabajo
                    {
                        Id = trabajoId,
                        IdVehiculo = Convert.ToInt32(reader["VehiculoId"]),
                        Observaciones = reader["Observaciones"].ToString(),
                        FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaEntrega"))
    ? null
    : Convert.ToDateTime(reader["FechaEntrega"]),
                        Estado = reader["Estado"].ToString(),
                        KilometrajeSalida = reader.IsDBNull(reader.GetOrdinal("KilometrajeSalida"))
        ? 0
        : Convert.ToInt32(reader["KilometrajeSalida"]),  // ✅ aquí
                        Vehiculo = new Vehiculo
                        {
                            Id = Convert.ToInt32(reader["VehiculoId"]),
                            Patente = reader["Patente"].ToString(),
                            Marca = reader["Marca"].ToString(),
                            Modelo = reader["Modelo"].ToString()
                        },
                        ServiciosRealizados = new List<TrabajoServicio>()
                    };


                    lista.Add(trabajoActual);
                }

                // ✅ Agregar el servicio del trabajo
                trabajoActual.ServiciosRealizados.Add(new TrabajoServicio
                {
                    ServicioId = Convert.ToInt32(reader["ServicioId"]),
                    Servicio = new Servicio
                    {
                        Id = Convert.ToInt32(reader["ServicioId"]),
                        Nombre = reader["ServicioNombre"].ToString()
                    },
                    CostoAplicado = Convert.ToDecimal(reader["CostoAplicado"]),
                    //  Comentario = reader["Comentario"].ToString()
                });
            }

            return lista;
        }



    }
}
