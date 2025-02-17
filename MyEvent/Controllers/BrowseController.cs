using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyEvent.DTOs;
using MyEvent.Models;
using MyEvent.ViewModels;

namespace MyEvent.Controllers
{
    public class BrowseController : Controller
    {
        private readonly MyEventContext _context;

        public BrowseController(MyEventContext context)
        {
            _context = context;
        }

        // GET: Browse
        public async Task<IActionResult> Index(IEnumerable<string>? EventTypeID, IEnumerable<string>? Region, DateOnly StartDate, DateOnly EndDate)
        {
            var events = _context.Event.Include(h => h.EventHolder).Include(t => t.EventType).Include(v => v.Venue).AsQueryable();



            if (EventTypeID != null && EventTypeID.Any())
            {
                events = events.Where(e => EventTypeID.Contains(e.EventType.EventTypeID));
            }
            

            if (Region != null && Region.Any())
            {
                events = events.Where(e => Region.Contains(e.Venue.Region));
            }
            if (StartDate != DateOnly.MinValue && EndDate != DateOnly.MinValue)
            {
                events = events.Where(e => e.Date >= StartDate && e.Date <= EndDate);
            }

            var result = await events.OrderByDescending(b => b.Date).ToListAsync();


            ViewBag.EventType = new SelectList(await _context.EventType.ToListAsync(), "EventTypeID", "EventType1");

            ViewBag.Region = new SelectList(_context.Venue, "Region", "Region");

            ViewBag.IsDarkbg = true;
            return View(result);
        }

        // GET: Browse/Details/5
        public async Task<IActionResult> Details(string id)
        {
            HttpContext.Session.SetString("EventID", id);

            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(h => h.EventHolder)
                .Include(t => t.EventType)
                .Include(v => v.Venue)
                .Include(a => a.EventTag)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.IsDarkbg = true;
            return View(@event);
        }

      
    }
}
