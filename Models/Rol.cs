namespace TallerMecanico.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación 
        public List<Usuario>? Usuarios { get; set; }
    }
}
