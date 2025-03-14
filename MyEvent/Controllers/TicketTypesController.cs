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
    public class TicketTypesController : Controller
    {
        private readonly MyEventContext _context;

        public TicketTypesController(MyEventContext context)
        {
            _context = context;
        }

        // GET: TicketTypes
        public async Task<IActionResult> Index()
        {
            var myEventContext = _context.TicketType.Include(t => t.Event).Include(t => t.TicketTypeList);
            return View(await myEventContext.ToListAsync());
        }

        // GET: TicketTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string? eventId = HttpContext.Session.GetString("EventID");
            var ticketType = await _context.TicketType
                .Include(t => t.Event)
                .Include(t => t.TicketTypeList)
                .Where(t => t.EventID == eventId)
                .ToListAsync();


            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // GET: TicketTypes/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID");
            ViewData["TicketTypeID"] = new SelectList(_context.TicketType, "TicketTypeID", "TicketTypeID");
            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,TicketTypeID")] TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", ticketType.EventID);
            ViewData["TicketTypeID"] = new SelectList(_context.TicketType, "TicketTypeID", "TicketTypeID", ticketType.TicketTypeID);
            return View(ticketType);
        }

        // GET: TicketTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketType.FindAsync(id);
            if (ticketType == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", ticketType.EventID);
            ViewData["TicketTypeID"] = new SelectList(_context.TicketType, "TicketTypeID", "TicketTypeID", ticketType.TicketTypeID);
            return View(ticketType);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EventID,TicketTypeID")] TicketType ticketType)
        {
            if (id != ticketType.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTypeExists(ticketType.EventID))
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
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", ticketType.EventID);
            ViewData["TicketTypeID"] = new SelectList(_context.TicketType, "TicketTypeID", "TicketTypeID", ticketType.TicketTypeID);
            return View(ticketType);
        }

        // GET: TicketTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketType
                .Include(t => t.Event)
                .Include(t => t.TicketTypeList)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ticketType = await _context.TicketType.FindAsync(id);
            if (ticketType != null)
            {
                _context.TicketType.Remove(ticketType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketTypeExists(string id)
        {
            return _context.TicketType.Any(e => e.EventID == id);
        }
    }
}
