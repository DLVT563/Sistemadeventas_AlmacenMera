namespace Sistemadeventas_AlmacenMera.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdCategoria { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaVencimiento { get; set; }

        public Proveedor Proveedor { get; set; }
        public Categoria Categoria { get; set; }
        public ICollection<DetalleVenta> DetallesVentas { get; set; }
        public ICollection<HistorialPrecio> HistorialPrecios { get; set; }
        public ICollection<HistorialEntradaSalida> HistorialEntradasSalidas { get; set; }
    }

}
