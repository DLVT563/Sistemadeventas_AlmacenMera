using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemadeventas_AlmacenMera.Data;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UsuariosController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var inventario2Context = _context.Usuarios.Include(u => u.IdRolNavigation);
            return View(await inventario2Context.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Email,Contraseña,IdRol,FechaCreacion,Estado")] Usuario usuario, IFormFile? fotoPerfil)
        {
            if (ModelState.IsValid)
            {
                if (fotoPerfil != null && fotoPerfil.Length > 0)
                {
                    usuario.FotoPerfilPath = await GuardarArchivoAsync(fotoPerfil, "usuarios");
                }
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Email,Contraseña,IdRol,FechaCreacion,Estado")] Usuario usuario, IFormFile? nuevaFoto)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioExistente = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);
                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    if (nuevaFoto != null && nuevaFoto.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(usuarioExistente.FotoPerfilPath))
                        {
                            EliminarArchivo(usuarioExistente.FotoPerfilPath);
                        }

                        usuario.FotoPerfilPath = await GuardarArchivoAsync(nuevaFoto, "usuarios");
                    }
                    else
                    {
                        usuario.FotoPerfilPath = usuarioExistente.FotoPerfilPath;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    ActualizarSesionSiCorresponde(usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                if (!string.IsNullOrEmpty(usuario.FotoPerfilPath))
                {
                    EliminarArchivo(usuario.FotoPerfilPath);
                }

                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        private async Task<string> GuardarArchivoAsync(IFormFile archivo, string carpeta)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", carpeta);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await archivo.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", carpeta, fileName).Replace("\\", "/");
        }

        private void EliminarArchivo(string relativePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        private void ActualizarSesionSiCorresponde(Usuario usuario)
        {
            var usuarioSesionId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioSesionId.HasValue && usuarioSesionId.Value == usuario.IdUsuario)
            {
                HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("EmailUsuario", usuario.Email);

                if (!string.IsNullOrEmpty(usuario.FotoPerfilPath))
                {
                    HttpContext.Session.SetString("FotoUsuario", usuario.FotoPerfilPath);
                }
                else
                {
                    HttpContext.Session.Remove("FotoUsuario");
                }
            }
        }
    }
}
