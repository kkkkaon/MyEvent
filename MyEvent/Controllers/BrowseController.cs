using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.DTOs;
using MyEvent.Models;

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
        public async Task<IActionResult> Index(string? EventType, string? Area, DateOnly StartDate, DateOnly EndDate)
        {

            //VMtStudent students = new VMtStudent()
            //{
            //    //這裡的名字要跟類別裡定義的屬性名稱相同
            //    Students = db.tStudent.Where(s => s.DeptID == deptid).ToList(),
            //    Departments = db.Department.ToList()
            //};

            var events = _context.Event.Include(h => h.EventHolder).Include(t => t.EventType).Include(v => v.Venue).AsQueryable();

            if (!string.IsNullOrEmpty(EventType))
            {
                events = events.Where(e => e.EventType.EventType1.Contains(EventType));
            }
            if (!string.IsNullOrEmpty(Area))
            {
                events = events.Where(e => e.Venue.Area.Contains(Area));
            }
            if (StartDate != DateOnly.MinValue && EndDate != DateOnly.MinValue)
            {
                events = events.Where(e => e.Date >= StartDate && e.Date <= EndDate);
            }
            



            return View(await events.Select(p => ItemEvent(p)).ToListAsync());
        }

        // GET: Browse/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(h => h.EventHolder)
                .Include(t => t.EventType)
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        private static EventDTO ItemEvent(Event e)
        {
            var result = new EventDTO //4.2.3 上面也要改 IEnumerable<ProductDTO>
            {
                EventID = e.EventID,
                EventName = e.EventName,
                Price = e.Price,
                Description = e.Description,
                Date = e.Date,
                VenueName = e.Venue.VenueName,
                EventHolderName = e.EventHolder.EventHolderName,
                EventType1 = e.EventType.EventType1
            };

            return result;
        }
    }
}
