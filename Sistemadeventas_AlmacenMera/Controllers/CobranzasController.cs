using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POSVentaMera.Models;
using Sistemadeventas_AlmacenMera.Data;

namespace Sistemadeventas_AlmacenMera.Controllers
{
    public class CobranzasController : Controller
    {
        private readonly AppDbContext _context;

        public CobranzasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? contribuyenteId, string? estado, int? mes, int? anio)
        {
            var query = _context.Cobranzas
                .Include(c => c.Contribuyente)
                .AsNoTracking()
                .AsQueryable();

            if (contribuyenteId.HasValue)
            {
                query = query.Where(c => c.IdContribuyente == contribuyenteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(c => c.Estado == estado);
            }

            if (mes.HasValue)
            {
                query = query.Where(c => c.Periodo.Month == mes.Value);
            }

            if (anio.HasValue)
            {
                query = query.Where(c => c.Periodo.Year == anio.Value);
            }

            var cobranzas = await query
                .OrderByDescending(c => c.Periodo)
                .ThenBy(c => c.Contribuyente.NombreContribuyente)
                .ToListAsync();

            ViewBag.Contribuyentes = new SelectList(await _context.Contribuyentes
                .OrderBy(c => c.NombreContribuyente)
                .ToListAsync(), "IdContribuyente", "NombreContribuyente", contribuyenteId);

            ViewBag.Estado = estado;
            ViewBag.Mes = mes;
            ViewBag.Anio = anio;

            ViewBag.ResumenPendiente = cobranzas.Where(c => c.Estado == "Pendiente").Sum(c => c.Monto);
            ViewBag.ResumenPagado = cobranzas.Where(c => c.Estado == "Pagado").Sum(c => c.Monto);

            return View(cobranzas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobranza = await _context.Cobranzas
                .Include(c => c.Contribuyente)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCobranza == id);
            if (cobranza == null)
            {
                return NotFound();
            }

            return View(cobranza);
        }

        public async Task<IActionResult> Create(int? contribuyenteId)
        {
            await PrepararSelectList(contribuyenteId);
            var modelo = new Cobranza
            {
                Periodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                IdContribuyente = contribuyenteId ?? 0
            };
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cobranza cobranza)
        {
            if (ModelState.IsValid)
            {
                cobranza.Periodo = new DateTime(cobranza.Periodo.Year, cobranza.Periodo.Month, 1);
                _context.Add(cobranza);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cobro registrado correctamente.";
                return RedirectToAction(nameof(Index), new { contribuyenteId = cobranza.IdContribuyente });
            }

            await PrepararSelectList(cobranza.IdContribuyente);
            return View(cobranza);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobranza = await _context.Cobranzas.FindAsync(id);
            if (cobranza == null)
            {
                return NotFound();
            }

            await PrepararSelectList(cobranza.IdContribuyente);
            return View(cobranza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cobranza cobranza)
        {
            if (id != cobranza.IdCobranza)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cobranza.Periodo = new DateTime(cobranza.Periodo.Year, cobranza.Periodo.Month, 1);
                    _context.Update(cobranza);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cobro actualizado.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CobranzaExists(cobranza.IdCobranza))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index), new { contribuyenteId = cobranza.IdContribuyente });
            }

            await PrepararSelectList(cobranza.IdContribuyente);
            return View(cobranza);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cobranza = await _context.Cobranzas
                .Include(c => c.Contribuyente)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCobranza == id);
            if (cobranza == null)
            {
                return NotFound();
            }

            return View(cobranza);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cobranza = await _context.Cobranzas.FindAsync(id);
            if (cobranza != null)
            {
                _context.Cobranzas.Remove(cobranza);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cobro eliminado.";
                return RedirectToAction(nameof(Index), new { contribuyenteId = cobranza.IdContribuyente });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarEstado(int id, string estado)
        {
            var cobranza = await _context.Cobranzas.FindAsync(id);
            if (cobranza == null)
            {
                return NotFound();
            }

            cobranza.Estado = estado;
            if (estado == "Pagado")
            {
                cobranza.FechaPago ??= DateTime.Now.Date;
            }
            else if (estado == "Pendiente")
            {
                cobranza.FechaPago = null;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Estado de la cobranza actualizado.";
            return RedirectToAction(nameof(Index), new { contribuyenteId = cobranza.IdContribuyente });
        }

        private async Task PrepararSelectList(int? contribuyenteId)
        {
            var contribuyentes = await _context.Contribuyentes
                .OrderBy(c => c.NombreContribuyente)
                .ToListAsync();
            ViewBag.IdContribuyente = new SelectList(contribuyentes, "IdContribuyente", "NombreContribuyente", contribuyenteId);
        }

        private bool CobranzaExists(int id)
        {
            return _context.Cobranzas.Any(e => e.IdCobranza == id);
        }
    }
}
