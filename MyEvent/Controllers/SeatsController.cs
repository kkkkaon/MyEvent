using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.Controllers
{
    public class SeatsController : Controller
    {
        private readonly MyEventContext _context;

        public SeatsController(MyEventContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Select()
        {
            string? eventId = HttpContext.Session.GetString("EventID");
            var eventItem = await _context.Event.Where(e => e.EventID == eventId).FirstOrDefaultAsync();

            var seats = await _context.Seat
                .Where(s => s.VenueID == eventItem.VenueID)
                .ToListAsync();

            if (eventItem == null)
            {
                throw new Exception($"⚠ 查無 EventID 為 {eventId} 的活動，請確認資料庫中是否存在該活動");
            }

            if (eventItem.VenueID == null)
            {
                throw new Exception("⚠ eventItem.VenueID 為 null，請確認 Event 資料是否有正確關聯 Venue");
            }

            var tt = await _context.TicketType.Where(t => t.EventID == eventId).Include(t => t.TicketTypeList).ToListAsync();
            ViewBag.TicketTypes = tt;

            ViewBag.EventID = eventId;
            ViewBag.VenueID = eventItem.VenueID;
            return View(seats);

        }



        public IActionResult LoadDiscount(string SeatID)
        { 
            return ViewComponent("VCDiscount", new { seatID = SeatID });
        }

        // GET: Seats
        public async Task<IActionResult> Index()
        {
            var myEventContext = _context.Seat.Include(s => s.Venue);
            return View(await myEventContext.ToListAsync());
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Venue)
                .FirstOrDefaultAsync(m => m.SeatID == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create
        public IActionResult Create()
        {
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueID");
            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeatID,SeatType,Price,Row,Number,VenueID")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueID", seat.VenueID);
            return View(seat);
        }

        // GET: Seats/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seat.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueID", seat.VenueID);
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SeatID,SeatType,Price,Row,Number,VenueID")] Seat seat)
        {
            if (id != seat.SeatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.SeatID))
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
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueID", seat.VenueID);
            return View(seat);
        }

        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seat
                .Include(s => s.Venue)
                .FirstOrDefaultAsync(m => m.SeatID == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var seat = await _context.Seat.FindAsync(id);
            if (seat != null)
            {
                _context.Seat.Remove(seat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(string id)
        {
            return _context.Seat.Any(e => e.SeatID == id);
        }
    }
}
