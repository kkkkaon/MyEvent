using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Ticket
{
    public string TicketID { get; set; } = null!;
    public string EventID { get; set; } = null!;

    public int Status { get; set; }

    public string TicketTypeID { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
    public virtual TicketType TicketType { get; set; } = null!;

}
