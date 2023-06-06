#nullable disable
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
    public class CoordinatorsController : Controller
    {
        private readonly CoordinatorsContext _context;

        public CoordinatorsController(CoordinatorsContext context)
        {
            _context = context;
        }

        private List<SelectListItem> GetPageSizes(int selectedPageSize = 5)
        {
            var pagesSizes = new List<SelectListItem>();

            if (selectedPageSize == 5)
                pagesSizes.Add(new SelectListItem("5", "5", true));
            else
                pagesSizes.Add(new SelectListItem("5", "5"));

            for (int lp = 10; lp <= 100; lp += 10)
            {
                if (lp == selectedPageSize)
                { pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true)); }
                else
                    pagesSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }

            return pagesSizes;
        }

        // GET: Coordinators
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<Coordinators> coordinators;

            if (pg < 1) pg = 1;


            if (SearchText != "" && SearchText != null)
            {
                coordinators = _context.Coordinators
                .Where(p => p.CoordinatorName.Contains(SearchText))
                .ToList();
            }
            else
                coordinators = _context.Coordinators.ToList();

            int recsCount = coordinators.Count();

            int recSkip = (pg - 1) * pageSize;

            List<Coordinators> retCoordinators = coordinators.Skip(recSkip).Take(pageSize).ToList();
            Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Coordinators", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;

            this.ViewBag.PageSizes = GetPageSizes(pageSize);

            return View(retCoordinators.ToList());

        }

        // GET: Coordinators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinators = await _context.Coordinators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinators == null)
            {
                return NotFound();
            }

            return View(coordinators);
        }

        // GET: Coordinators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoordinatorName")] Coordinators coordinators)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinators);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coordinators);
        }

        // GET: Coordinators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinators = await _context.Coordinators.FindAsync(id);
            if (coordinators == null)
            {
                return NotFound();
            }
            return View(coordinators);
        }

        // POST: Coordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoordinatorName")] Coordinators coordinators)
        {
            if (id != coordinators.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinators);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinatorsExists(coordinators.Id))
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
            return View(coordinators);
        }

        // GET: Coordinators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinators = await _context.Coordinators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinators == null)
            {
                return NotFound();
            }

            return View(coordinators);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coordinators == null)
            {
                return Problem("Entity set 'CoordinatorsContext.Coordinators'  is null.");
            }
            var coordinators = await _context.Coordinators.FindAsync(id);
            if (coordinators != null)
            {
                _context.Coordinators.Remove(coordinators);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinatorsExists(int id)
        {
          return (_context.Coordinators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
