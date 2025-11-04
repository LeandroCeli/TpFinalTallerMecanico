namespace TallerMecanico.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }   // FK con Cliente
        public string Patente { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;


        // Nuevos campos
        public int Kilometraje { get; set; }
        public string Tipo { get; set; } = string.Empty; // Ej: "Personal", "Trabajo", "Otro"

        // Propiedad de navegación
        public Cliente? Cliente { get; set; }
        
    }
}
