namespace Sistemadeventas_AlmacenMera.Models
{
    public class HistorialVentasUsuario
    {
        public int IdHistorialVenta { get; set; }
        public int IdVenta { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public decimal TotalVenta { get; set; }

        public Venta Venta { get; set; }
    }

}
