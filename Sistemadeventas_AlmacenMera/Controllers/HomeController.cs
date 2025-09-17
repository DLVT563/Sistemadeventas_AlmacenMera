using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            return View();
        }

        public IActionResult Privacy()
        {
            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetInt32("UsuarioId").HasValue;
        }
    }
}
