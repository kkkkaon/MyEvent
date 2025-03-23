using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyEvent.Models;
using System.Diagnostics;
using GoodStore.Filters;

namespace MyEvent.Controllers
{
    [ServiceFilter(typeof(EHLoginFilter))]
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
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1");
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event event1, IFormFile? newPhoto)
        {
            var eventID = event1.Date.ToString("yyyyMMdd");
            var eventDay = event1.Date.Day;
            var eventYear = event1.Date.Year;
            var eventMonth = event1.Date.Month;

            var lastEvent = _context.Event.Where(m => m.Date.Year == eventYear && m.Date.Month == eventMonth && m.Date.Day == eventDay).OrderByDescending(m => m.EventID).FirstOrDefault();
            var newEventID = lastEvent != null ? (int.Parse(lastEvent.EventID) + 1).ToString() : "E" + eventID + "01";
            event1.EventID = newEventID;

            if (newPhoto != null && newPhoto.Length > 0)
            {
                // **確保目錄存在**
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EventPhotos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // **生成唯一檔名 (避免覆蓋舊檔案)**
                string fileExtension = Path.GetExtension(newPhoto.FileName); // 取得副檔名
                string fileName = event1.EventID + fileExtension; // 確保檔名符合 EventID

                string filePath = Path.Combine(uploadsFolder, fileName);

                // **刪除舊檔案，避免累積垃圾檔案**
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // **儲存新檔案**
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await newPhoto.CopyToAsync(stream);
                }

                // **更新圖片名稱**
                event1.Pic = fileName;
            }

            var id = HttpContext.Session.GetString("EventHolderID");
            event1.EventHolderID = id;


            ModelState.Remove("EventHolderID");
            ModelState.Remove("EventID");
            ModelState.Remove("Pic");

            if (ModelState.IsValid)
            {
                _context.Add(event1);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EHLogin");
            }
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", event1.EventTypeID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", event1.VenueID);
            return View(event1);
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
            ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", @event.EventTypeID);
            ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Event @event, IFormFile? newPhoto)
        {
            if (id != @event.EventID)
            {
                return NotFound();
            }

            var existingEvent = await _context.Event.AsNoTracking().FirstOrDefaultAsync(e => e.EventID == id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            if (newPhoto != null && newPhoto.Length > 0)
            {
                // **允許的檔案格式 (忽略大小寫)**
                var allowedFormats = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/jpg", "image/png" };
                if (!allowedFormats.Contains(newPhoto.ContentType))
                {
                    ViewData["Message"] = "請選擇 JPG/PNG 檔案上傳";
                    ViewData["EventTypeID"] = new SelectList(_context.EventType, "EventTypeID", "EventType1", @event.EventTypeID);
                    ViewData["VenueID"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);
                    return View(@event);
                }

                // **確保目錄存在**
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EventPhotos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // **生成唯一檔名 (避免覆蓋舊檔案)**
                string fileExtension = Path.GetExtension(newPhoto.FileName); // 取得副檔名
                string fileName = @event.EventID + fileExtension; // 確保檔名符合 EventID

                string filePath = Path.Combine(uploadsFolder, fileName);

                // **刪除舊檔案，避免累積垃圾檔案**
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // **儲存新檔案**
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await newPhoto.CopyToAsync(stream);
                }

                // **更新圖片名稱**
                @event.Pic = fileName;
            }
            else
            {
                // **如果使用者沒上傳新照片，保留原本的照片**
                @event.Pic = existingEvent.Pic;
            }
            var EHID = HttpContext.Session.GetString("EventHolderID");
            @event.EventHolderID = EHID;

            ModelState.Remove("EventHolderID");
            ModelState.Remove("Pic");

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
                return RedirectToAction("Index", "EHLogin");
            }
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
            return RedirectToAction("Index", "EHLogin");
        }

        private bool EventExists(string id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}
