using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class TicketType
{
    public string TypeID { get; set; } = null!;

    public string Name { get; set; } = null!;

    public double Discount { get; set; }

    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
}
