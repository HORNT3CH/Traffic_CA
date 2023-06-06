#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Traffic_CA.Data;
using Traffic_CA.Models;

namespace Traffic_Hudson.Controllers
{
    public class DocklotsController : Controller
    {
        private readonly DocklotsContext _context;

        public DocklotsController(DocklotsContext context)
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

        // GET: Docklot
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<Docklot> docklot;

            if (pg < 1) pg = 1;


            if (SearchText != "" && SearchText != null)
            {
                if (DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                {
                    var searchDates = DateTime.ParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    docklot = _context.Docklot
                    .Where(p => p.DocklotDate.Date == searchDates.Date)
                    .ToList();
                }
                else
                {
                    docklot = _context.Docklot
                    .Where(p => p.TrailerNumber.Contains(SearchText) || p.Location.Contains(SearchText) || p.CarrierName.Contains(SearchText) || p.Status.Contains(SearchText))
                    .ToList();
                }
            }
            else
                docklot = _context.Docklot.ToList();

            int recsCount = docklot.Count();

            int recSkip = (pg - 1) * pageSize;

            List<Docklot> retDocklot = docklot.Skip(recSkip).Take(pageSize).ToList();
            Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Docklots", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;

            this.ViewBag.PageSizes = GetPageSizes(pageSize);

            return View(retDocklot.ToList());

        }

        // GET: Docklots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Docklot == null)
            {
                return NotFound();
            }

            var docklot = await _context.Docklot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docklot == null)
            {
                return NotFound();
            }

            return View(docklot);
        }

        // GET: Docklots/CreateDock
        public IActionResult CreateDock()
        {
            ViewBag.Door = new SelectList(_context.Doors.ToList().Where(x => x.Status == "Open").OrderBy(x => x.Location), "Location", "Location");
            ViewBag.Carrier = new SelectList(_context.Carriers.ToList().OrderBy(m => m.CarrierName), "CarrierName", "CarrierName");
            return View();
        }

        // POST: Docklots/CreateDock
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDock([Bind("Id,CarrierName,TrailerNumber,Dimension,TrailerComments,LoadNbr,DocklotDate,Location,Status")] Docklot docklot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docklot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(docklot);
        }

        // GET: Docklots/CreateLot
        public IActionResult CreateLot()
        {
            ViewBag.Lot = new SelectList(_context.Lots.ToList().Where(x => x.Status == "Open").OrderBy(x => x.Location), "Location", "Location");
            ViewBag.Carrier = new SelectList(_context.Carriers.ToList().OrderBy(m => m.CarrierName), "CarrierName", "CarrierName");
            return View();
        }

        // POST: Docklots/CreateLot
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLot([Bind("Id,CarrierName,TrailerNumber,Dimension,TrailerComments,LoadNbr,DocklotDate,Location,Status")] Docklot docklot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docklot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(docklot);
        }

        // GET: Docklots/DockMove/5
        public async Task<IActionResult> DockMove(int? id)
        {
            ViewBag.Door = new SelectList(_context.Doors.ToList().Where(x => x.Status == "Open").OrderBy(x => x.Location), "Location", "Location");
            if (id == null || _context.Docklot == null)
            {
                return NotFound();
            }

            var docklot = await _context.Docklot.FindAsync(id);
            if (docklot == null)
            {
                return NotFound();
            }
            return View(docklot);
        }

        // POST: Docklots/DockMove/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DockMove(int id, [Bind("Id,CarrierName,TrailerNumber,Dimension,TrailerComments,LoadNbr,DocklotDate,Location,Status")] Docklot docklot)
        {
            if (id != docklot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docklot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocklotExists(docklot.Id))
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
            return View(docklot);
        }

        // GET: Docklots/LotMove/5
        public async Task<IActionResult> LotMove(int? id)
        {
            ViewBag.Lot = new SelectList(_context.Lots.ToList().Where(x => x.Status == "Open").OrderBy(x => x.Location), "Location", "Location");
            if (id == null || _context.Docklot == null)
            {
                return NotFound();
            }

            var docklot = await _context.Docklot.FindAsync(id);
            if (docklot == null)
            {
                return NotFound();
            }
            return View(docklot);
        }

        // POST: Docklots/LotMove/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LotMove(int id, [Bind("Id,CarrierName,TrailerNumber,Dimension,TrailerComments,LoadNbr,DocklotDate,Location,Status")] Docklot docklot)
        {
            if (id != docklot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docklot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocklotExists(docklot.Id))
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
            return View(docklot);
        }

        // GET: Docklots/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null || _context.Docklot == null)
            {
                return NotFound();
            }

            var docklot = await _context.Docklot.FindAsync(id);
            if (docklot == null)
            {
                return NotFound();
            }
            return View(docklot);
        }

        // POST: Docklots/ChangeStatus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, [Bind("Id,CarrierName,TrailerNumber,Dimension,TrailerComments,LoadNbr,DocklotDate,Location,Status")] Docklot docklot)
        {
            if (id != docklot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docklot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocklotExists(docklot.Id))
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
            return View(docklot);
        }

        // GET: Docklots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Docklot == null)
            {
                return NotFound();
            }

            var docklot = await _context.Docklot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docklot == null)
            {
                return NotFound();
            }

            return View(docklot);
        }

        // POST: Docklots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Docklot == null)
            {
                return Problem("Entity set 'DocklotsContext.Docklot'  is null.");
            }
            var docklot = await _context.Docklot.FindAsync(id);
            if (docklot != null)
            {
                _context.Docklot.Remove(docklot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocklotExists(int id)
        {
            return (_context.Docklot?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
