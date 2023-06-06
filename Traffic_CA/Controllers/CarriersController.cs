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
    public class CarriersController : Controller
    {
        private readonly CarriersContext _context;

        public CarriersController(CarriersContext context)
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

		// GET: Carriers
		public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
		{
			List<Carriers> carriers;

			if (pg < 1) pg = 1;


			if (SearchText != "" && SearchText != null)
			{
				carriers = _context.Carriers
				.Where(p => p.CarrierName.Contains(SearchText))
				.ToList();
			}
			else
				carriers = _context.Carriers.ToList();

			int recsCount = carriers.Count();

			int recSkip = (pg - 1) * pageSize;

			List<Carriers> retCarriers = carriers.Skip(recSkip).Take(pageSize).ToList();
			Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Carriers", SearchText = SearchText };
			ViewBag.SearchPager = SearchPager;

			this.ViewBag.PageSizes = GetPageSizes(pageSize);

			return View(retCarriers.ToList());

		}

		// GET: Carriers/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carriers == null)
            {
                return NotFound();
            }

            var carriers = await _context.Carriers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carriers == null)
            {
                return NotFound();
            }

            return View(carriers);
        }

        // GET: Carriers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carriers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CarrierName,CarrierSTCC,CarrierContact,CarrierPhone,CarrierFax,CarrierEmail")] Carriers carriers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carriers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carriers);
        }

        // GET: Carriers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carriers == null)
            {
                return NotFound();
            }

            var carriers = await _context.Carriers.FindAsync(id);
            if (carriers == null)
            {
                return NotFound();
            }
            return View(carriers);
        }

        // POST: Carriers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarrierName,CarrierSTCC,CarrierContact,CarrierPhone,CarrierFax,CarrierEmail")] Carriers carriers)
        {
            if (id != carriers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carriers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarriersExists(carriers.Id))
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
            return View(carriers);
        }

        // GET: Carriers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carriers == null)
            {
                return NotFound();
            }

            var carriers = await _context.Carriers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carriers == null)
            {
                return NotFound();
            }

            return View(carriers);
        }

        // POST: Carriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carriers == null)
            {
                return Problem("Entity set 'CarriersContext.Carriers'  is null.");
            }
            var carriers = await _context.Carriers.FindAsync(id);
            if (carriers != null)
            {
                _context.Carriers.Remove(carriers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarriersExists(int id)
        {
          return (_context.Carriers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
