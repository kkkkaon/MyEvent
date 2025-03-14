using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using MyEvent.ViewComponents;
using MyEvent.ViewModels;

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
                string eventId = HttpContext.Session.GetString("EventID");

                return RedirectToAction("Details", new { eventId = eventId }); // 若無座位選擇，重新導向
            }

            var seatIds = SeatID.Select(x => x).ToList();
            var ticketTypeIds = TicketTypeID.Select(x => x).ToList();

            var seatList = await _context.Seat
                                      .Where(t => seatIds.Contains(t.SeatID))
                                      .ToListAsync();
            //var ticketTypeList = await _context.TicketTypeList
            //                          .Where(t => ticketTypeIds.Contains(t.TicketTypeID))
            //                          .ToListAsync();

            var result = new List<dynamic>();
            for (int i = 0; i < seatList.Count; i++)
            {
                var seat = seatList.FirstOrDefault(t => t.SeatID == seatIds[i]);
                var ticketType = await _context.TicketTypeList.FirstOrDefaultAsync(t => t.TicketTypeID == ticketTypeIds[i]);

                if (ticketType != null && seat!=null)
                {
                    result.Add(new
                    {
                        TicketTypeID = ticketType.TicketTypeID,
                        TicketTypeName = ticketType.Name,
                        discount = Convert.ToDecimal(ticketType.Discount),
                        Qty = 1,
                        price = Convert.ToDecimal(seat.Price),
                        SeatID = seat.SeatID,
                        SeatRow = seat.Row,
                        SeatNum = seat.Number
                    });
                }
            }

            return View(result);
        }

        //給不分區的confirm
        public async Task<IActionResult> ConfirmQty(List<string> TicketTypeID, List<string> qty, string SeatID)
        {
            var seat = await _context.Seat.FirstOrDefaultAsync(s => s.SeatID == SeatID);
            if (seat == null)
            {
                // 如果找不到對應的座位資料，則返回錯誤頁面或處理
                return NotFound();
            }

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
                        TicketTypeID = ticketType.TicketTypeID,
                        TicketTypeName = ticketType.Name,
                        discount = Convert.ToDecimal(ticketType.Discount),
                        Qty = Convert.ToDecimal(quantities[i]),
                        price = Convert.ToDecimal(seat.Price),
                        SeatID = seat.SeatID
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
