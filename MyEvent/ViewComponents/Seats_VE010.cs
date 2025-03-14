using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.ViewComponents
{
    public class Seats_VE010 : ViewComponent
    {
        private readonly MyEventContext _context;

        public Seats_VE010(MyEventContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string venueId)
        {
            if (string.IsNullOrEmpty(venueId))
            {
                return View("Error"); 
            }

            var seats = await _context.Seat
                .Where(s => s.VenueID == venueId)
                .ToListAsync();

            return View(seats); 
        }
    }

}
