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
    public class DefaultTimesController : Controller
    {
        private readonly DefaultTimesContext _context;

        public DefaultTimesController(DefaultTimesContext context)
        {
            _context = context;
        }

        // GET: DefaultTimes
        public async Task<IActionResult> Index()
        {
              return _context.DefaultTimes != null ? 
                          View(await _context.DefaultTimes.ToListAsync()) :
                          Problem("Entity set 'DefaultTimesContext.DefaultTimes'  is null.");
        }

        // GET: DefaultTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DefaultTimes == null)
            {
                return NotFound();
            }

            var defaultTimes = await _context.DefaultTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defaultTimes == null)
            {
                return NotFound();
            }

            return View(defaultTimes);
        }

        // GET: DefaultTimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DefaultTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,Standard,SlotDate,Scheduled,OverRide,ReceivingStandard,ReceivingScheduled,ReceivingOverRide")] DefaultTimes defaultTimes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(defaultTimes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(defaultTimes);
        }

        // GET: DefaultTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DefaultTimes == null)
            {
                return NotFound();
            }

            var defaultTimes = await _context.DefaultTimes.FindAsync(id);
            if (defaultTimes == null)
            {
                return NotFound();
            }
            return View(defaultTimes);
        }

        // POST: DefaultTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,Standard,SlotDate,Scheduled,OverRide,ReceivingStandard,ReceivingScheduled,ReceivingOverRide")] DefaultTimes defaultTimes)
        {
            if (id != defaultTimes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(defaultTimes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefaultTimesExists(defaultTimes.Id))
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
            return View(defaultTimes);
        }

        // GET: DefaultTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DefaultTimes == null)
            {
                return NotFound();
            }

            var defaultTimes = await _context.DefaultTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (defaultTimes == null)
            {
                return NotFound();
            }

            return View(defaultTimes);
        }

        // POST: DefaultTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DefaultTimes == null)
            {
                return Problem("Entity set 'DefaultTimesContext.DefaultTimes'  is null.");
            }
            var defaultTimes = await _context.DefaultTimes.FindAsync(id);
            if (defaultTimes != null)
            {
                _context.DefaultTimes.Remove(defaultTimes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefaultTimesExists(int id)
        {
          return (_context.DefaultTimes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
