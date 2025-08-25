namespace Sistemadeventas_AlmacenMera.Models
{
    public class HistorialPrecio
    {
        public int IdPrecio { get; set; }
        public int IdProducto { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCambio { get; set; } = DateTime.Now;

        public Producto Producto { get; set; }
    }

}
