namespace Sistemadeventas_AlmacenMera.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }

}
