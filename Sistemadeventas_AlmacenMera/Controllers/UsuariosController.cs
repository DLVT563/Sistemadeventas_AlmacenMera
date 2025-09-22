using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Usuarios.Include(u => u.IdRolNavigation);
            return View(await appDbContext.ToListAsync());
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Email,Contraseña,IdRol,FechaCreacion,Estado,FotoPerfilPath")] Usuario usuario, IFormFile? FotoPerfilFile)
        {
            if (ModelState.IsValid)
            {
                // Manejo de imagen de perfil
                if (FotoPerfilFile != null && FotoPerfilFile.Length > 0)
                {
                    // Ruta absoluta de wwwroot/usuariosImg
                    string uploadFolder = Path.Combine("wwwroot", "usuariosImg");

                    // Crear carpeta si no existe
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Nombre único para evitar colisiones
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FotoPerfilFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    // Guardar archivo físicamente en wwwroot/usuariosImg
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoPerfilFile.CopyToAsync(stream);
                    }

                    // Guardar en BD la ruta relativa (para usar en <img>)
                    usuario.FotoPerfilPath = "/usuariosImg/" + fileName;
                }

                // Asignar fecha de creación
                usuario.FechaCreacion = DateTime.Now;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Email,Contraseña,IdRol,FechaCreacion,Estado,FotoPerfilPath")] Usuario usuario, IFormFile? FotoPerfilFile)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Manejo de imagen de perfil (si se ha subido una nueva imagen)
                    if (FotoPerfilFile != null && FotoPerfilFile.Length > 0)
                    {
                        // Ruta absoluta de wwwroot/usuariosImg
                        string uploadFolder = Path.Combine("wwwroot", "usuariosImg");

                        // Crear carpeta si no existe
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        // Nombre único para evitar colisiones
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FotoPerfilFile.FileName);
                        string filePath = Path.Combine(uploadFolder, fileName);

                        // Guardar archivo físicamente en wwwroot/usuariosImg
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await FotoPerfilFile.CopyToAsync(stream);
                        }

                        // Guardar en BD la ruta relativa (para usar en <img>)
                        usuario.FotoPerfilPath = "/usuariosImg/" + fileName;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
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
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
