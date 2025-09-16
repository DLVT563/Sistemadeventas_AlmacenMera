namespace Sistemadeventas_AlmacenMera.Models
{
    public class HistorialEntradaSalida
    {
        public int IdHistorial { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;
        public string Observaciones { get; set; }

        public Producto Producto { get; set; }
    }

}
