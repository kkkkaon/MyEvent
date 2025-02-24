using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using MyEvent.ViewComponents;

namespace MyEvent.Controllers
{
    public class OrderController : Controller
    {
        private readonly MyEventContext _context;

        public OrderController(MyEventContext context)
        {
            _context = context;
        }

        //給有座位的confirm
        public async Task<IActionResult> Confirm(List<string> SeatID, List<string> TicketTypeID)
        {
            if (SeatID == null || TicketTypeID == null || SeatID.Count == 0)
            {
                return RedirectToAction("Select"); // 若無座位選擇，重新導向
            }

            // 假設有 Seat 和 TicketType 的資料庫表
            var selectedSeats = await  _context.Seat
                                        .Where(s => SeatID.Contains(s.SeatID))
                                        .ToListAsync();

            var ticketTypes = await _context.TicketTypeList
                                      .Where(t => TicketTypeID.Contains(t.TicketTypeID))
                                      .ToListAsync();

            // 組合座位與票種資訊
            var orderDetails = selectedSeats.Select((seat, index) => new
            {
                Seat = seat,
                TicketType = ticketTypes.ElementAtOrDefault(index) // 確保索引不超出範圍
            }).ToList();

            return View(orderDetails);
        }

        //給不分區的confirm
        public async Task<IActionResult> ConfirmQty(List<string> TicketTypeID, List<string> qty, decimal Price)
        {
            var ticketTypeIds = TicketTypeID.Select(x => x).ToList();
            var quantities = qty.Select(x => x).ToList();

            // 取得對應的票種資料
            var ticketTypeList = await _context.TicketTypeList
                                      .Where(t => ticketTypeIds.Contains(t.TicketTypeID))
                                      .ToListAsync();

            // 假設傳進來的兩個 List 順序一致，
            // 組合票種資訊與數量的匿名物件列表
            var result = new List<dynamic>();
            for (int i = 0; i < ticketTypeIds.Count; i++)
            {
                var ticketType = ticketTypeList.FirstOrDefault(t => t.TicketTypeID == ticketTypeIds[i]);
                if (ticketType != null)
                {
                    result.Add(new
                    {
                        TicketTypeName = ticketType.Name,  
                        discount = Convert.ToDecimal(ticketType.Discount),  
                        Qty = Convert.ToDecimal(quantities[i]),
                        price = Convert.ToDecimal(Price)
                    });
                }
            }

            return View(result);
        }
        // GET: Order
        public async Task<IActionResult> Index()
        {
            var myEventContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Seat).Include(o => o.Ticket);
            return View(await myEventContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Seat)
                .Include(o => o.Ticket)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID");
            ViewData["SeatID"] = new SelectList(_context.Seat, "SeatID", "SeatID");
            ViewData["TicketID"] = new SelectList(_context.Ticket, "TicketID", "TicketID");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,SeatID,TicketID,Price")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["SeatID"] = new SelectList(_context.Seat, "SeatID", "SeatID", orderDetail.SeatID);
            ViewData["TicketID"] = new SelectList(_context.Ticket, "TicketID", "TicketID", orderDetail.TicketID);
            return View(orderDetail);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["SeatID"] = new SelectList(_context.Seat, "SeatID", "SeatID", orderDetail.SeatID);
            ViewData["TicketID"] = new SelectList(_context.Ticket, "TicketID", "TicketID", orderDetail.TicketID);
            return View(orderDetail);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,SeatID,TicketID,Price")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderID))
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
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["SeatID"] = new SelectList(_context.Seat, "SeatID", "SeatID", orderDetail.SeatID);
            ViewData["TicketID"] = new SelectList(_context.Ticket, "TicketID", "TicketID", orderDetail.TicketID);
            return View(orderDetail);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Seat)
                .Include(o => o.Ticket)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetail.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(string id)
        {
            return _context.OrderDetail.Any(e => e.OrderID == id);
        }
    }
}
