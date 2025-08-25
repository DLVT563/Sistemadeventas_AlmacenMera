namespace Sistemadeventas_AlmacenMera.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public int? IdRol { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "activo";

        public Rol Rol { get; set; }
        public ICollection<Venta> Ventas { get; set; }
    }

}
