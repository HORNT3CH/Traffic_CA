#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Traffic_CA.Data;
using Traffic_CA.Models;

namespace Traffic_Hudson.Controllers
{
    public class LoadsController : Controller
    {
        private readonly LoadsContext _context;

        public LoadsController(LoadsContext context)
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
        // GET: Loads
        public IActionResult Index(string SearchText = "", int pg = 1, int pageSize = 5)
        {
            List<Loads> loads;

            if (pg < 1) pg = 1;


            if (SearchText != "" && SearchText != null)
            {
                if (DateTime.TryParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                {
                    var searchDates = DateTime.ParseExact(SearchText, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    loads = _context.Loads
                    .Where(p => p.OrderDate.Date == searchDates.Date && p.LoadType != "HOTLTL")
                    .OrderBy(p => p.OrderDate).ThenBy(p => p.ConvertedTimeSlots)
                    .ToList();
                }
                else
                {
                    loads = _context.Loads
                    .Where(p => p.MBOLNbr.Contains(SearchText) || p.LoadNbr.Contains(SearchText) || p.CustomerName.Contains(SearchText) || p.CarrierName.Contains(SearchText) || p.LoadStatus == SearchText && p.LoadType != "HOTLTL")
                    .OrderBy(p => p.OrderDate).ThenBy(p => p.ConvertedTimeSlots)
                    .ToList();
                }
            }
            else
                loads = _context.Loads.Where(p => p.LoadType != "HOTLTL").ToList();

            int recsCount = loads.Count();

            int recSkip = (pg - 1) * pageSize;

            List<Loads> retLoads = loads.Skip(recSkip).Take(pageSize).ToList();
            Pager SearchPager = new Pager(recsCount, pg, pageSize) { Action = "Index", Controller = "Loads", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;

            this.ViewBag.PageSizes = GetPageSizes(pageSize);
            ViewBag.Customer = new SelectList(_context.Customers.ToList().OrderBy(m => m.CustomerName), "CustomerName", "CustomerName");

            return View(retLoads.ToList());

        }

        public async Task<IActionResult> IndexLTL()
        {
            return _context.Loads != null ?
                        View(await _context.Loads.Where(p => p.LoadType == "HOTLTL" && p.LoadStatus == "Created").ToListAsync()) :
                        Problem("Entity set 'LoadsContext.Loads'  is null.");
        }


        // GET: Loads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loads == null)
            {
                return NotFound();
            }

            return View(loads);
        }

        public JsonResult UpdateTimeSlots(string selectedDate)
        {
            var timeSlots = _context.DefaultTimes
                .Where(m => m.Scheduled < (m.Standard + m.OverRide) && m.SlotDate.Date == DateTime.Parse(selectedDate).Date)
                .OrderBy(m => m.Id)
                .Select(m => new { StartTime = m.StartTime })
                .ToList();

            return Json(timeSlots);
        }

        // GET: Loads/Create
        public IActionResult Create()
        {
            ViewBag.TimeSlot = new SelectList(_context.DefaultTimes.ToList().Where(m => m.Scheduled < (m.Standard + m.OverRide) && ((Func<DateTime, DateTime, int>)((d1, d2) => DateTime.Compare(d1.Date, d2.Date)))(m.SlotDate, DateTime.Now) == 0).OrderBy(m => m.Id), "StartTime", "StartTime");
            ViewBag.Carrier = new SelectList(_context.Carriers.ToList().OrderBy(m => m.CarrierName), "CarrierName", "CarrierName");
            ViewBag.City = new SelectList(_context.Cities.ToList().OrderBy(m => m.CityName), "CityName", "CityName");
            ViewBag.Coordinator = new SelectList(_context.Coordinators.ToList().OrderBy(m => m.CoordinatorName), "CoordinatorName", "CoordinatorName");
            ViewBag.Customer = new SelectList(_context.Customers.ToList().OrderBy(m => m.CustomerName), "CustomerName", "CustomerName");
            ViewBag.State = new SelectList(_context.States.ToList().OrderBy(m => m.StateName), "StateName", "StateName");
            return View();
        }

        // POST: Loads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loads);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Load " + loads.MBOLNbr + " Was Scheduled On " + loads.OrderDate.ToShortDateString() + " At " + loads.TimeSlot + "!";
                return RedirectToAction("Index", "Loads");
            }
            return View(loads);
        }

        public async Task<IActionResult> StageSheet(int? Id)
        {
            var loads = await _context.Loads.FindAsync(Id);
            return View(loads);
        }

        // GET: Loads/Stage/5
        public async Task<IActionResult> Stage(int? id)
        {
            ViewBag.Stagers = new SelectList(_context.Associate.ToList().Where(m => m.Department == "Shipping").OrderBy(m => m.AssociateName), "AssociateName", "AssociateName");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Stage/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stage(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Load " + loads.MBOLNbr + " has been updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(loads);
        }

        // GET: Loads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Loaders = new SelectList(_context.Associate.ToList().Where(m => m.Department == "Shipping").OrderBy(m => m.AssociateName), "AssociateName", "AssociateName");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexLTL));
            }
            return View(loads);
        }

        // GET: Loads/Reschedule/5
        public async Task<IActionResult> Reschedule(int? id)
        {
            ViewBag.TimeSlot = new SelectList(_context.DefaultTimes.ToList().Where(m => m.Scheduled < (m.Standard + m.OverRide) && ((Func<DateTime, DateTime, int>)((d1, d2) => DateTime.Compare(d1.Date, d2.Date)))(m.SlotDate, DateTime.Now) == 0).OrderBy(m => m.Id), "StartTime", "StartTime");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
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
            return View(loads);
        }

        // GET: Loads/Load/5
        public async Task<IActionResult> LoadLive(int? id)
        {
            ViewBag.Loaders = new SelectList(_context.Associate.ToList().Where(m => m.Department == "Shipping").OrderBy(m => m.AssociateName), "AssociateName", "AssociateName");
            ViewBag.Door = new SelectList(_context.Doors.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            ViewBag.Lot = new SelectList(_context.Lots.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            var carrierName = loads.CarrierName;
            ViewBag.Docklot = new SelectList(_context.Docklot.ToList().Where(m => m.Status == "Empty" && m.CarrierName == carrierName).OrderBy(m => m.TrailerNumber), "TrailerNumber", "TrailerNumber");
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Load/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadLive(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Load " + loads.MBOLNbr + " has been updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(loads);
        }

        // GET: Loads/Unload/5
        public async Task<IActionResult> UnloadLive(int? id)
        {
            ViewBag.Unloaders = new SelectList(_context.Associate.ToList().Where(m => m.Department == "Shipping").OrderBy(m => m.AssociateName), "AssociateName", "AssociateName");
            ViewBag.Door = new SelectList(_context.Doors.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            ViewBag.Lot = new SelectList(_context.Lots.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            var carrierName = loads.CarrierName;
            ViewBag.Docklot = new SelectList(_context.Docklot.ToList().Where(m => m.Status == "Empty" && m.CarrierName == carrierName).OrderBy(m => m.TrailerNumber), "TrailerNumber", "TrailerNumber");
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Load/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnloadLive(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Load " + loads.MBOLNbr + " has been updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(loads);
        }

        // GET: Loads/Load/5
        public async Task<IActionResult> LoadSpot(int? id)
        {
            ViewBag.Loaders = new SelectList(_context.Associate.ToList().Where(m => m.Department == "Shipping").OrderBy(m => m.AssociateName), "AssociateName", "AssociateName");
            ViewBag.Door = new SelectList(_context.Doors.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            ViewBag.Lot = new SelectList(_context.Lots.ToList().Where(m => m.Status == "Open").OrderBy(m => m.Location), "Location", "Location");
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads.FindAsync(id);
            var carrierName = loads.CarrierName;
            ViewBag.Docklot = new SelectList(_context.Docklot.ToList().Where(m => m.Status == "Empty" && m.CarrierName == carrierName).OrderBy(m => m.TrailerNumber), "TrailerNumber", "TrailerNumber");
            if (loads == null)
            {
                return NotFound();
            }
            return View(loads);
        }

        // POST: Loads/Load/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadSpot(int id, [Bind("Id,OrderDate,LoadNbr,TimeSlot,StageLocation,CarrierName,LoadType,CustomerName,City,State,MBOLNbr,StagerOne,StagerTwo,StagerThree,StageStart,StageFinish,NbrCartons,Comments,LoadCube,NbrStops,Waved,CoordinatorName,LoadStatus,Labels,LoaderOne,LoaderTwo,LoaderThree,LoadStart,LoadFinish,TrailerNumber,Lot,Door,ArrivalTime,RequestedTrailer,AppointmentNumber,PickupDate,LoadValue")] Loads loads)
        {
            if (id != loads.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loads);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadsExists(loads.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = "Load " + loads.MBOLNbr + " has been updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(loads);
        }

        // GET: Loads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Loads == null)
            {
                return NotFound();
            }

            var loads = await _context.Loads
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loads == null)
            {
                return NotFound();
            }

            return View(loads);
        }

        // Load parameter page for common carrier
        public ActionResult GetDockLotPrint()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDockLot()
        {
            string connectionString = "Data Source=MGAE-447;Database=TrafficBELive_DEV;User Id=dbreader;Password=dbreader;";
            using (SqlConnection connection = new(connectionString))
            {
                using (SqlCommand command = new("sp_GetDockLot", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<GetDockLot> getDockLot = new();
                        while (reader.Read())
                        {
                            getDockLot.Add(new GetDockLot
                            {
                                Id = reader.GetInt32(0),
                                Location = reader.GetString(1),
                                CarrierName = reader.GetString(2),
                                TrailerNumber = reader.GetString(3),
                                Status = reader.GetString(4),
                                TrailerComments = reader.GetString(5),
                                MBOLNbr = reader.GetString(6),
                                DocklotDate = reader.GetDateTime(7)
                            });
                        }

                        return View(getDockLot);
                    }
                }
            }
        }

        // POST: Loads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Loads == null)
            {
                return Problem("Entity set 'LoadsContext.Loads'  is null.");
            }
            var loads = await _context.Loads.FindAsync(id);
            if (loads != null)
            {
                _context.Loads.Remove(loads);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoadsExists(int id)
        {
            return (_context.Loads?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
