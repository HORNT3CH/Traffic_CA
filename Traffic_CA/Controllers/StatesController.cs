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
    public class StatesController : Controller
    {
        private readonly StatesContext _context;

        public StatesController(StatesContext context)
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

        // GET: States
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<States> states;

            if (pg < 1) pg = 1;


            if (SearchText != "" && SearchText != null)
            {
                states = _context.States
                .Where(p => p.StateName.Contains(SearchText))
                .ToList();
            }
            else
                states = _context.States.ToList();

            int recsCount = states.Count();

            int recSkip = (pg - 1) * pageSize;

            List<States> retStates = states.Skip(recSkip).Take(pageSize).ToList();
            Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "States", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;

            this.ViewBag.PageSizes = GetPageSizes(pageSize);

            return View(retStates.ToList());

        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var states = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
            if (states == null)
            {
                return NotFound();
            }

            return View(states);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StateName")] States states)
        {
            if (ModelState.IsValid)
            {
                _context.Add(states);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(states);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var states = await _context.States.FindAsync(id);
            if (states == null)
            {
                return NotFound();
            }
            return View(states);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StateName")] States states)
        {
            if (id != states.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(states);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatesExists(states.Id))
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
            return View(states);
        }

        // GET: States/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.States == null)
            {
                return NotFound();
            }

            var states = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
            if (states == null)
            {
                return NotFound();
            }

            return View(states);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.States == null)
            {
                return Problem("Entity set 'StatesContext.States'  is null.");
            }
            var states = await _context.States.FindAsync(id);
            if (states != null)
            {
                _context.States.Remove(states);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatesExists(int id)
        {
          return (_context.States?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
