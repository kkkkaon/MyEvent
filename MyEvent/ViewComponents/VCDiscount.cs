using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.ViewComponents
{
    public class VCDiscount : ViewComponent
    {
        private readonly MyEventContext _context;

        public VCDiscount(MyEventContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string seatID)
        {
            string eventId = HttpContext.Session.GetString("EventID");
            var discounts = await _context.TicketType.Where(t => t.EventID == eventId).Include(t => t.TicketTypeList).ToListAsync();

            var seat = await _context.Seat.Where(o => o.SeatID == seatID).FirstOrDefaultAsync();
            ViewBag.Seat = seat;


            var event1 = await _context.Event.Where(t => t.EventID == eventId).FirstOrDefaultAsync();
            var seatType = await _context.Seat.Where(t => t.VenueID == event1.VenueID).ToListAsync();

            if (seatType.Count() == 1){
                
                return View("Qty",discounts);
            }

            return View(discounts);

        }
    }
}
