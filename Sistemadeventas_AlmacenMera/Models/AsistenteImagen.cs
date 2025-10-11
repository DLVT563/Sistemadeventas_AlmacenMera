using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models
{
    public class AsistenteImagen
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DescripcionDetallada { get; set; } = string.Empty;
        public string DescripcionBreve { get; set; } = string.Empty;
        public string RutaImagen { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Etiquetas { get; set; } = new List<string>();
    }
}
