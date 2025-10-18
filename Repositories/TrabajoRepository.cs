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
                            Descripcion = reader.GetString("descripcion"),
                            FechaInicio = reader.GetDateTime("fecha_inicio"),
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
                                Descripcion = reader.GetString("descripcion"),
                                FechaInicio = reader.GetDateTime("fecha_inicio"),
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
                var sql = @"INSERT INTO trabajos (id_vehiculo, descripcion, fecha_inicio, fecha_fin, costo_total, estado)
                            VALUES (@vehiculo, @desc, @inicio, @fin, @costo, @estado);
                            SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@vehiculo", trabajo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@desc", trabajo.Descripcion);
                    cmd.Parameters.AddWithValue("@inicio", trabajo.FechaInicio);
                    cmd.Parameters.AddWithValue("@fin", (object?)trabajo.FechaFin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@costo", trabajo.CostoTotal);
                    cmd.Parameters.AddWithValue("@estado", trabajo.Estado);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
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
                            fecha_inicio=@inicio,
                            fecha_fin=@fin,
                            costo_total=@costo,
                            estado=@estado
                            WHERE id_trabajo=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@vehiculo", trabajo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@desc", trabajo.Descripcion);
                    cmd.Parameters.AddWithValue("@inicio", trabajo.FechaInicio);
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
    }
}
