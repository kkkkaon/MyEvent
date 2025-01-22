using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Payment
{
    public int PaymentID { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
