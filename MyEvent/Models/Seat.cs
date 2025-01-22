using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Seat
{
    public string SeatID { get; set; } = null!;

    public string SeatType { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Row { get; set; }

    public string? Number { get; set; }

    public string VenueID { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual Venue Venue { get; set; } = null!;
}
