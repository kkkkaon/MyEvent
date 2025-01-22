using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.ViewComponents
{
    public class VCEventsTopFour : ViewComponent
    {
        private readonly MyEventContext _context;

        public VCEventsTopFour(MyEventContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var events = await _context.Event.OrderByDescending(b => b.Date).Take(4).ToListAsync();
            return View(events);
        }
    }
}
