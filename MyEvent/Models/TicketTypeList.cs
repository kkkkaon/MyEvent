using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class TicketTypeList
{
    public string TicketTypeID { get; set; } = null!;

    public string Name { get; set; } = null!;

    public double Discount { get; set; }

    public virtual ICollection<TicketType> TicketType { get; set; } = new List<TicketType>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
