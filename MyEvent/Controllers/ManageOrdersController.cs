using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodStore.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.Controllers
{
    public class ManageOrdersController : Controller
    {
        private readonly MyEventContext _context;
        public ManageOrdersController(MyEventContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(AdminLoginFilter))]
        // GET: ManageOrders
        public async Task<IActionResult> Index()
        {
            var myEventContext = await _context.Order.Include(o => o.Event).Include(o => o.Member).OrderByDescending(o => o.Date).ToListAsync();
            return View( myEventContext);
        }

        // GET: ManageOrders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Event)
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: ManageOrders/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID");
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID");
            ViewData["PaymentID"] = new SelectList(_context.Payment, "PaymentID", "PaymentID");
            return View();
        }

        // POST: ManageOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Date,Qty,Status,Memo,EventID,MemberID,PaymentID,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", order.EventID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberID", order.MemberID);
            return View(order);
        }

        // GET: ManageOrders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Event)
                .Include(o => o.Member)
                .Include(o => o.OrderDetail)
                .FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", order.EventID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName", order.Member.MemberID);
            //ViewData["PaymentID"] = new SelectList(_context.Payment, "PaymentID", "PaymentID", order.PaymentID);
            return View(order);
        }

        // POST: ManageOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["EventID"] = new SelectList(_context.Event, "EventID", "EventID", order.EventID);
            ViewData["MemberID"] = new SelectList(_context.Member, "MemberID", "MemberName", order.MemberID);
            //ViewData["PaymentID"] = new SelectList(_context.Payment, "PaymentID", "PaymentID", order.PaymentID);
            return View(order);
        }

        //[ServiceFilter(typeof(AdminLoginFilter))]
        // GET: ManageOrders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Event)
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: ManageOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
