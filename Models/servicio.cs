public class Servicio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public decimal CostoBase { get; set; }

    public bool IncluyeInsumos { get; set; }

    // Navegación inversa (opcional)
 // public List<TrabajoServicio> ServiciosRealizados { get; set; } = new();
}