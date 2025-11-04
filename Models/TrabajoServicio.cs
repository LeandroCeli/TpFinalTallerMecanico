public class TrabajoServicio
{
    public int Id { get; set; }
    public int TrabajoId { get; set; }
    public int ServicioId { get; set; }
    public decimal CostoAplicado { get; set; }

    // Opcional: navegación
    public Servicio? Servicio { get; set; }
}