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

            // 取得所有已經存在於 OrderDetail 的 SeatID（代表已被訂購）
            var bookedSeatIds = await _context.OrderDetail
                .Select(od => od.SeatID)
                .ToListAsync();

            bool isUpdated = false; // 記錄是否有需要更新的座位

            foreach (var seat in seats)
            {
                if (!bookedSeatIds.Contains(seat.SeatID) && seat.Status != "1")
                {
                    seat.Status = "1"; // 若座位未被訂單使用，則標記為可選擇
                    isUpdated = true;
                }
            }

            // 只有在有變更時才更新資料庫，避免不必要的 SaveChanges()
            if (isUpdated)
            {
                await _context.SaveChangesAsync();
            }

            return View(seats);

        }
    }

}
