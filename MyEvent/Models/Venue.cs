using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Venue
{
    public string VenueID { get; set; } = null!;

    public string VenueName { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();

    public virtual ICollection<Seat> Seat { get; set; } = new List<Seat>();
}
