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
    public class TimeSlotsController : Controller
    {
        private readonly TimeSlotsContext _context;

        public TimeSlotsController(TimeSlotsContext context)
        {
            _context = context;
        }

        // GET: TimeSlots
        public async Task<IActionResult> Index()
        {
              return _context.TimeSlots != null ? 
                          View(await _context.TimeSlots.ToListAsync()) :
                          Problem("Entity set 'TimeSlotsContext.TimeSlots'  is null.");
        }

        // GET: TimeSlots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TimeSlots == null)
            {
                return NotFound();
            }

            var timeSlots = await _context.TimeSlots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSlots == null)
            {
                return NotFound();
            }

            return View(timeSlots);
        }

        // GET: TimeSlots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeSlots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TimeSlot")] TimeSlots timeSlots)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeSlots);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeSlots);
        }

        // GET: TimeSlots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TimeSlots == null)
            {
                return NotFound();
            }

            var timeSlots = await _context.TimeSlots.FindAsync(id);
            if (timeSlots == null)
            {
                return NotFound();
            }
            return View(timeSlots);
        }

        // POST: TimeSlots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TimeSlot")] TimeSlots timeSlots)
        {
            if (id != timeSlots.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeSlots);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeSlotsExists(timeSlots.Id))
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
            return View(timeSlots);
        }

        // GET: TimeSlots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TimeSlots == null)
            {
                return NotFound();
            }

            var timeSlots = await _context.TimeSlots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeSlots == null)
            {
                return NotFound();
            }

            return View(timeSlots);
        }

        // POST: TimeSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TimeSlots == null)
            {
                return Problem("Entity set 'TimeSlotsContext.TimeSlots'  is null.");
            }
            var timeSlots = await _context.TimeSlots.FindAsync(id);
            if (timeSlots != null)
            {
                _context.TimeSlots.Remove(timeSlots);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeSlotsExists(int id)
        {
          return (_context.TimeSlots?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
