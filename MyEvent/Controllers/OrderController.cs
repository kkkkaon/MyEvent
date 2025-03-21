using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodStore.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(MemberLoginFilter))]
        public async Task<IActionResult> Confirm(List<string> SeatID, List<string> TicketTypeID)
        {

            var seatIds = SeatID.Select(x => x).ToList();
            var ticketTypeIds = TicketTypeID.Select(x => x).ToList();

            var seatList = await _context.Seat
                                      .Where(t => seatIds.Contains(t.SeatID))
                                      .ToListAsync();

            ViewBag.QtyTotal = seatList.Count;
            var result = new List<dynamic>();
            for (int i = 0; i < seatList.Count; i++)
            {
                var seat = seatList.FirstOrDefault(t => t.SeatID == seatIds[i]);
                var ticketType = await _context.TicketTypeList.FirstOrDefaultAsync(t => t.TicketTypeID == ticketTypeIds[i]);

                if (ticketType != null && seat!=null)
                {
                    seat.Status = "2";
                    result.Add(new
                    {
                        TicketTypeID = ticketType.TicketTypeID,
                        TicketTypeName = ticketType.Name,
                        discount = Convert.ToDecimal(ticketType.Discount),
                        price = Convert.ToDecimal(seat.Price),
                        SeatID = seat.SeatID,
                        SeatRow = seat.Row,
                        SeatNum = seat.Number
                    });
                }
            }


            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(MemberLoginFilter))]
        public async Task<IActionResult> PlaceOrder(int QtyTotal, string EventID, string MemberID, decimal TotalPrice, string OrderDetails)
        {

            ModelState.Remove("OrderID");
            ModelState.Remove("MemberID");

            if (ModelState.IsValid)
            {
                try
                {
                    //呼叫寫在dbcontext2的SP
                    var result = await _context.ExecSPAddNewOrderAsync(QtyTotal, EventID, MemberID, TotalPrice, OrderDetails);

                    //_context.Add(order);
                    //await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        TempData["OrderMessage"] = "OK";
                        return RedirectToAction("Index","Memberlogin");
                    }
                }
                catch(Exception ex)
                {
                    string eventId1 = HttpContext.Session.GetString("EventID");

                    return RedirectToAction("Details", "Browse", new { id = eventId1 });
                }
            }
            string eventId = HttpContext.Session.GetString("EventID");

            return RedirectToAction("Details", "Browse", new { id = eventId });
        }

        //給不分區的confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(MemberLoginFilter))]
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
            //var myEventContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Seat);
            var myEventContext = _context.Order.Include(e=>e.Event).Include(m=>m.Member);
            return View(await myEventContext.ToListAsync());
        }


        private bool OrderDetailExists(string id)
        {
            return _context.OrderDetail.Any(e => e.OrderID == id);
        }
    }
}
