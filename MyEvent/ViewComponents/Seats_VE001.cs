using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.ViewComponents
{
    public class Seats_VE001 : ViewComponent
    {
        private readonly MyEventContext _context;

        public Seats_VE001(MyEventContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                return View("Error"); // 這裡處理當 venueId 不存在的情況
            }

            var seats = await _context.Seat
                .Where(s => s.VenueID == venueId)
                .ToListAsync();

            return View(seats); // 傳遞座位資料
        }
    }

}
