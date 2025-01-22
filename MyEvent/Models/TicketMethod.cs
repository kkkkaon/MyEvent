using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class TicketMethod
{
    public int MethodID { get; set; }

    public string Method { get; set; } = null!;

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
