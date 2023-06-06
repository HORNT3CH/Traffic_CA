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
    public class AssociatesController : Controller
    {
        private readonly AssociatesContext _context;

        public AssociatesController(AssociatesContext context)
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

        // GET: Associates
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<Associate> associate;

            if (pg < 1) pg = 1;


            if (SearchText != "" && SearchText != null)
            {
                associate = _context.Associate
                .Where(p => p.AssociateName.Contains(SearchText))
                .ToList();
            }
            else
                associate = _context.Associate.ToList();

            int recsCount = associate.Count();

            int recSkip = (pg - 1) * pageSize;

            List<Associate> retAssociate = associate.Skip(recSkip).Take(pageSize).ToList();
            Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Associates", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;

            this.ViewBag.PageSizes = GetPageSizes(pageSize);

            return View(retAssociate.ToList());

        }

        // GET: Associates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Associate == null)
            {
                return NotFound();
            }

            var associate = await _context.Associate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (associate == null)
            {
                return NotFound();
            }

            return View(associate);
        }

        // GET: Associates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Associates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssociateName,AssociateInitials,ShiftNumber,Department,AssociateUserName")] Associate associate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(associate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(associate);
        }

        // GET: Associates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Associate == null)
            {
                return NotFound();
            }

            var associate = await _context.Associate.FindAsync(id);
            if (associate == null)
            {
                return NotFound();
            }
            return View(associate);
        }

        // POST: Associates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssociateName,AssociateInitials,ShiftNumber,Department,AssociateUserName")] Associate associate)
        {
            if (id != associate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(associate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociateExists(associate.Id))
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
            return View(associate);
        }

        // GET: Associates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Associate == null)
            {
                return NotFound();
            }

            var associate = await _context.Associate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (associate == null)
            {
                return NotFound();
            }

            return View(associate);
        }

        // POST: Associates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Associate == null)
            {
                return Problem("Entity set 'AssociatesContext.Associate'  is null.");
            }
            var associate = await _context.Associate.FindAsync(id);
            if (associate != null)
            {
                _context.Associate.Remove(associate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssociateExists(int id)
        {
          return (_context.Associate?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
