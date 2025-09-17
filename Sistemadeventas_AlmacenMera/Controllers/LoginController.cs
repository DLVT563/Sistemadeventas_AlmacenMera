using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemadeventas_AlmacenMera.Data;
using Sistemadeventas_AlmacenMera.ViewModel;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UsuarioId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Contraseña == model.Contraseña);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas. Verifique su correo y contraseña.");
                model.Contraseña = string.Empty;
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetString("EmailUsuario", usuario.Email);

            if (usuario.IdRol.HasValue)
            {
                HttpContext.Session.SetInt32("RolUsuario", usuario.IdRol.Value);
            }
            else
            {
                HttpContext.Session.Remove("RolUsuario");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
