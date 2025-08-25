namespace Sistemadeventas_AlmacenMera.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }

}
