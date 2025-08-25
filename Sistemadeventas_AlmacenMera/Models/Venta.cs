namespace Sistemadeventas_AlmacenMera.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "pendiente";
        public string TipoVenta { get; set; } = "contado";

        public Usuario Usuario { get; set; }
        public ICollection<DetalleVenta> DetallesVentas { get; set; }
        public Fiado Fiado { get; set; }
    }

}
