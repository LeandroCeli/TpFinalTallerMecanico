namespace TallerMecanico.Models
{
    public class Trabajo
    {
        public int Id { get; set; }
        public int IdVehiculo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal CostoTotal { get; set; }
        public string Estado { get; set; } // Ej: "En Proceso", "Finalizado", "Entregado"

        // Navegación
        public Vehiculo? Vehiculo { get; set; }
    }
}
