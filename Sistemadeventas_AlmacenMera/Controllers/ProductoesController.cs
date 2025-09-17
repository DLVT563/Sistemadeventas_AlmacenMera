using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Sistemadeventas_AlmacenMera.Data;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductoesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            var inventario2Context = _context.Productos.Include(p => p.IdCategoriaNavigation).Include(p => p.IdProveedorNavigation);
            return View(await inventario2Context.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias.OrderBy(c => c.NombreCategoria), "IdCategoria", "NombreCategoria");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores.OrderBy(p => p.NombreProveedor), "IdProveedor", "NombreProveedor");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,NombreProducto,Descripcion,Precio,Stock,CodigoBarras,IdProveedor,IdCategoria,FechaCreacion,FechaVencimiento")] Producto producto, IFormFile? fotoProducto)
        {
            if (ModelState.IsValid)
            {
                if (fotoProducto != null && fotoProducto.Length > 0)
                {
                    producto.FotoPath = await GuardarArchivoAsync(fotoProducto, "productos");
                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias.OrderBy(c => c.NombreCategoria), "IdCategoria", "NombreCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores.OrderBy(p => p.NombreProveedor), "IdProveedor", "NombreProveedor", producto.IdProveedor);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias.OrderBy(c => c.NombreCategoria), "IdCategoria", "NombreCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores.OrderBy(p => p.NombreProveedor), "IdProveedor", "NombreProveedor", producto.IdProveedor);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,NombreProducto,Descripcion,Precio,Stock,CodigoBarras,IdProveedor,IdCategoria,FechaCreacion,FechaVencimiento")] Producto producto, IFormFile? nuevaFoto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.IdProducto == id);
                    if (productoExistente == null)
                    {
                        return NotFound();
                    }

                    if (nuevaFoto != null && nuevaFoto.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(productoExistente.FotoPath))
                        {
                            var rutaAnterior = Path.Combine(_environment.WebRootPath, productoExistente.FotoPath);
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                        }

                        producto.FotoPath = await GuardarArchivoAsync(nuevaFoto, "productos");
                    }
                    else
                    {
                        producto.FotoPath = productoExistente.FotoPath;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias.OrderBy(c => c.NombreCategoria), "IdCategoria", "NombreCategoria", producto.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores.OrderBy(p => p.NombreProveedor), "IdProveedor", "NombreProveedor", producto.IdProveedor);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                if (!string.IsNullOrEmpty(producto.FotoPath))
                {
                    var ruta = Path.Combine(_environment.WebRootPath, producto.FotoPath);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }

                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
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
    }
}
