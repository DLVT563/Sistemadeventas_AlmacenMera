using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemadeventas_AlmacenMera.Data;
using Sistemadeventas_AlmacenMera.Models;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Productos.Include(p => p.IdCategoriaNavigation).Include(p => p.IdProveedorNavigation);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NombreProveedor");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,NombreProducto,Descripcion,Precio,Stock,IdProveedor,IdCategoria,FechaVencimiento")] Productos productos, IFormFile? FotoFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Generar código de barras aleatorio (12 dígitos)
                productos.CodigoBarras = GenerarCodigoBarras();

                // 2. Fecha de creación
                productos.FechaCreacion = DateTime.Now;

                // 3. Manejo de imagen
                if (FotoFile != null && FotoFile.Length > 0)
                {
                    // Ruta absoluta de wwwroot/productosImg
                    string uploadFolder = Path.Combine("wwwroot", "productosImg");

                    // Crear carpeta si no existe
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // Nombre único para evitar colisiones
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FotoFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);

                    // Guardar archivo físicamente en wwwroot/productosImg
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoFile.CopyToAsync(stream);
                    }

                    // Guardar en BD la ruta relativa (para usar en <img>)
                    productos.FotoPath = "/productosImg/" + fileName;
                }

                _context.Add(productos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", productos.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NombreProveedor", productos.IdProveedor);
            return View(productos);
        }


        // Método auxiliar para generar código de barras aleatorio
        private string GenerarCodigoBarras()
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, 12).Select(_ => random.Next(0, 10).ToString()));
        }


        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", productos.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NombreProveedor", productos.IdProveedor);
            return View(productos);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,NombreProducto,Descripcion,Precio,Stock,CodigoBarras,FotoPath,IdProveedor,IdCategoria,FechaCreacion,FechaVencimiento")] Productos productos)
        {
            if (id != productos.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductosExists(productos.IdProducto))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NombreCategoria", productos.IdCategoria);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NombreProveedor", productos.IdProveedor);
            return View(productos);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productos == null)
            {
                return NotFound();
            }

            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productos = await _context.Productos.FindAsync(id);
            if (productos != null)
            {
                _context.Productos.Remove(productos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductosExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
