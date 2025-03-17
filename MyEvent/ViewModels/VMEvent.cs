using MyEvent.Models;

namespace MyEvent.ViewModels
{
    public class VMEvent
    {
        public List<Event>? Events { get; set; }
        public List<EventHolder>? EventHolders { get; set; }
        public List<Venue>? Venues { get; set; }

    }

}
