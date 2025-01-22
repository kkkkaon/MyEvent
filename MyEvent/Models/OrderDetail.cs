using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class OrderDetail
{
    public string OrderID { get; set; } = null!;

    public string SeatID { get; set; } = null!;

    public string TicketID { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
