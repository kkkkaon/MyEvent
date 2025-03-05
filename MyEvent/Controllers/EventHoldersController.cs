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
    public class EventHoldersController : Controller
    {
        private readonly MyEventContext _context;

        public EventHoldersController(MyEventContext context)
        {
            _context = context;
        }

        // GET: EventHolders
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventHolder.ToListAsync());
        }

        // GET: EventHolders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHolder = await _context.EventHolder
                .FirstOrDefaultAsync(m => m.EventHolderID == id);
            if (eventHolder == null)
            {
                return NotFound();
            }

            return View(eventHolder);
        }

        // GET: EventHolders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventHolders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventHolderID,EventHolderName,JoinDate,Tel,ZipCode,City,Area,Address")] EventHolder eventHolder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventHolder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventHolder);
        }

        // GET: EventHolders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHolder = await _context.EventHolder.FindAsync(id);
            if (eventHolder == null)
            {
                return NotFound();
            }
            return View(eventHolder);
        }

        // POST: EventHolders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EventHolderID,EventHolderName,JoinDate,Tel,ZipCode,City,Area,Address")] EventHolder eventHolder)
        {
            if (id != eventHolder.EventHolderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventHolder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventHolderExists(eventHolder.EventHolderID))
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
            return View(eventHolder);
        }

        // GET: EventHolders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventHolder = await _context.EventHolder
                .FirstOrDefaultAsync(m => m.EventHolderID == id);
            if (eventHolder == null)
            {
                return NotFound();
            }

            return View(eventHolder);
        }

        // POST: EventHolders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var eventHolder = await _context.EventHolder.FindAsync(id);
            if (eventHolder != null)
            {
                _context.EventHolder.Remove(eventHolder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventHolderExists(string id)
        {
            return _context.EventHolder.Any(e => e.EventHolderID == id);
        }
    }
}
