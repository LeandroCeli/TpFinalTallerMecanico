namespace TallerMecanico.Models
{
    public class Trabajo
    {
        public int Id { get; set; }
        public int IdVehiculo { get; set; }

        public int UsuarioId { get; set; }

        public string Observaciones { get; set; }

        public DateTime? FechaFin { get; set; }
        public decimal CostoTotal { get; set; }
        public string Estado { get; set; } 
        public int KilometrajeSalida { get; set; }

        // Relación con servicios realizados
        public List<TrabajoServicio> ServiciosRealizados { get; set; } = new List<TrabajoServicio>();




        // Navegación
        public Vehiculo? Vehiculo { get; set; }
    }
}
