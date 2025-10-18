namespace TallerMecanico.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }   // FK con Cliente
        public string Patente { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Año { get; set; }
        public string Color { get; set; } = string.Empty;

        // Propiedad de navegación
        public Cliente? Cliente { get; set; }
    }
}
