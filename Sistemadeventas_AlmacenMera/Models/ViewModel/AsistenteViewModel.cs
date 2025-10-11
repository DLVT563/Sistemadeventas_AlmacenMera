using System.Collections.Generic;

namespace Sistemadeventas_AlmacenMera.Models.ViewModel
{
    public class AsistenteViewModel
    {
        public string NombreAsistente { get; set; } = "MerAI";
        public string MensajeInicial { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Recomendaciones { get; set; } = new List<string>();
        public IReadOnlyCollection<AsistenteImagen> ImagenesDestacadas { get; set; } = new List<AsistenteImagen>();
    }
}
