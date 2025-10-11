using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistemadeventas_AlmacenMera.Models;
using Sistemadeventas_AlmacenMera.Models.ViewModel;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class AsistenteController : Controller
    {
        private static readonly IReadOnlyCollection<AsistenteImagen> ImagenesPredefinidas = new List<AsistenteImagen>
        {
            new AsistenteImagen
            {
                Id = "frutas-frescas",
                Nombre = "Canasta de frutas frescas",
                DescripcionBreve = "Una selección colorida de frutas de temporada ideales para exhibiciones llamativas.",
                DescripcionDetallada = "Observamos una canasta de frutas que incluye manzanas rojas, plátanos, naranjas y uvas. Es perfecta para promocionar productos frescos en la sección de abarrotes y resaltar la variedad disponible.",
                RutaImagen = "img/asistente/frutas-frescas.svg",
                Etiquetas = new[] { "frutas", "abarrotes", "frescura", "vitaminas" }
            },
            new AsistenteImagen
            {
                Id = "lacteos",
                Nombre = "Refrigerador con lácteos",
                DescripcionBreve = "Anaquel refrigerado con leche, yogures y queso listos para tus clientes.",
                DescripcionDetallada = "Se aprecia un estante refrigerado con diferentes tipos de lácteos: botellas de leche, envases de yogur y bloques de queso. Es ideal para planificar promociones cruzadas con panadería o frutas.",
                RutaImagen = "img/asistente/lacteos.svg",
                Etiquetas = new[] { "lácteos", "refrigerado", "inventario", "promociones" }
            },
            new AsistenteImagen
            {
                Id = "limpieza",
                Nombre = "Kit de limpieza del hogar",
                DescripcionBreve = "Productos de limpieza esenciales para destacar combos de higiene.",
                DescripcionDetallada = "En la imagen encontramos detergente líquido, limpiadores multiusos y guantes de goma. Es excelente para sugerir paquetes promocionales para el hogar y campañas de temporada.",
                RutaImagen = "img/asistente/limpieza.svg",
                Etiquetas = new[] { "hogar", "limpieza", "promoción", "combo" }
            },
            new AsistenteImagen
            {
                Id = "granos",
                Nombre = "Sacos de granos básicos",
                DescripcionBreve = "Sacos de arroz, maíz y legumbres listos para el almacén.",
                DescripcionDetallada = "La escena muestra sacos apilados con granos secos como arroz y frijoles. Es útil para controlar existencias al mayoreo y planificar abastecimientos masivos.",
                RutaImagen = "img/asistente/granos.svg",
                Etiquetas = new[] { "granos", "inventario", "mayoreo", "almacén" }
            }
        };

        public IActionResult Index()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Login", "Login");
            }

            var viewModel = new AsistenteViewModel
            {
                MensajeInicial = "Hola, soy MerAI. Selecciona una imagen para darte una descripción útil para tu negocio.",
                Recomendaciones = new List<string>
                {
                    "Haz clic sobre una imagen para recibir una descripción detallada.",
                    "Puedes usar las etiquetas como inspiración para nuevas categorías o promociones.",
                    "Presiona \"Reiniciar conversación\" para comenzar de nuevo."
                },
                ImagenesDestacadas = ImagenesPredefinidas
            };

            return View(viewModel);
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId").HasValue;
        }
    }
}
