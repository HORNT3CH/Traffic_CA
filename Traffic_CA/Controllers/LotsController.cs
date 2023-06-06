using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Traffic_CA.Data;
using Traffic_CA.Models;

namespace Traffic_CA.Controllers
{
    public class LotsController : Controller
    {
        private readonly LotsContext _context;

        public LotsController(LotsContext context)
        {
            _context = context;
        }

        // GET: Lots
        public async Task<IActionResult> Index()
        {
              return _context.Lots != null ? 
                          View(await _context.Lots.ToListAsync()) :
                          Problem("Entity set 'LotsContext.Lots'  is null.");
        }

        // GET: Lots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lots == null)
            {
                return NotFound();
            }

            var lots = await _context.Lots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lots == null)
            {
                return NotFound();
            }

            return View(lots);
        }

        // GET: Lots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Location,Status")] Lots lots)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lots);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lots);
        }

        // GET: Lots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lots == null)
            {
                return NotFound();
            }

            var lots = await _context.Lots.FindAsync(id);
            if (lots == null)
            {
                return NotFound();
            }
            return View(lots);
        }

        // POST: Lots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Status")] Lots lots)
        {
            if (id != lots.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lots);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LotsExists(lots.Id))
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
            return View(lots);
        }

        // GET: Lots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lots == null)
            {
                return NotFound();
            }

            var lots = await _context.Lots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lots == null)
            {
                return NotFound();
            }

            return View(lots);
        }

        // POST: Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lots == null)
            {
                return Problem("Entity set 'LotsContext.Lots'  is null.");
            }
            var lots = await _context.Lots.FindAsync(id);
            if (lots != null)
            {
                _context.Lots.Remove(lots);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LotsExists(int id)
        {
          return (_context.Lots?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
