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
    public class EventsController : Controller
    {
        private readonly MyEventContext _context;

        public EventsController(MyEventContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var myEventContext = _context.Event.Include(e => e.EventHolder).Include(e => e.EventType).Include(e => e.Venue);
            return View(await myEventContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.EventHolder)
                .Include(e => e.EventType)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            
            ViewData["EventHolderID"] = new SelectList(_context.EventHolder, "EventHolderID", "EventHolderName");
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1");
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,EventName,Date,StartTime,VenueID,EventHolderID,Description,Pic,Price,Discount,Note,EventTypeID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventHolderID"] = new SelectList(_context.EventHolder, "EventHolderID", "EventHolderName", @event.EventHolderID);
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", @event.EventTypeID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["EventHolderID"] = new SelectList(_context.EventHolder, "EventHolderID", "EventHolderName", @event.EventHolderID);
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", @event.EventTypeID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EventID,EventName,Date,StartTime,VenueID,EventHolderID,Description,Pic,Price,Discount,Note,EventTypeID")] Event @event)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventID))
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
            ViewData["EventHolderID"] = new SelectList(_context.EventHolder, "EventHolderID", "EventHolderName", @event.EventHolderID);
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", @event.EventTypeID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.EventHolder)
                .Include(e => e.EventType)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(string id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}
