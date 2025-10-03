using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSVentaMera.Models;
using Sistemadeventas_AlmacenMera.Data;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class ContribuyentesController : Controller
    {
        private readonly AppDbContext _context;

        public ContribuyentesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? estado, string? search)
        {
            var query = _context.Contribuyentes
                .Include(c => c.Cobranzas)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.NombreContribuyente.Contains(search) || c.Ruc.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(c => c.Estatus == estado);
            }

            var contribuyentes = await query
                .OrderBy(c => c.NombreContribuyente)
                .ToListAsync();

            ViewBag.EstadoFiltro = estado;
            ViewBag.Search = search;

            return View(contribuyentes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribuyente = await _context.Contribuyentes
                .Include(c => c.Cobranzas)
                .ThenInclude(c => c.Contribuyente)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdContribuyente == id);

            if (contribuyente == null)
            {
                return NotFound();
            }

            contribuyente.Cobranzas = contribuyente.Cobranzas
                .OrderByDescending(c => c.Periodo)
                .ThenByDescending(c => c.IdCobranza)
                .ToList();

            return View(contribuyente);
        }

        public IActionResult Create()
        {
            return View(new Contribuyente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contribuyente contribuyente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contribuyente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Contribuyente registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(contribuyente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribuyente = await _context.Contribuyentes.FindAsync(id);
            if (contribuyente == null)
            {
                return NotFound();
            }

            return View(contribuyente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contribuyente contribuyente)
        {
            if (id != contribuyente.IdContribuyente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contribuyente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Contribuyente actualizado.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContribuyenteExists(contribuyente.IdContribuyente))
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

            return View(contribuyente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contribuyente = await _context.Contribuyentes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdContribuyente == id);
            if (contribuyente == null)
            {
                return NotFound();
            }

            return View(contribuyente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contribuyente = await _context.Contribuyentes.FindAsync(id);
            if (contribuyente != null)
            {
                _context.Contribuyentes.Remove(contribuyente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Contribuyente eliminado.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContribuyenteExists(int id)
        {
            return _context.Contribuyentes.Any(e => e.IdContribuyente == id);
        }
    }
}
