using System.ComponentModel.DataAnnotations;
using MyEvent.Models;

namespace MyEvent.DTOs
{
    public class EventDTO
    {
        public string EventID { get; set; } = null!;

        public string EventName { get; set; } = null!;

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public string VenueID { get; set; } = null!;

        public string VenueName { get; set; } = null!;

        public string Area { get; set; } = null!;

        public string EventHolderID { get; set; } = null!;

        public string EventHolderName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Pic { get; set; } = null!;

        public string Price { get; set; } = null!;

        public string? Discount { get; set; }

        public string? Note { get; set; }

        public string EventTypeID { get; set; } = null!;

        [Display(Name = "EventType")]
        public string EventType1 { get; set; } = null!;
    }
}
