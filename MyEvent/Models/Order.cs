using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Order
{
    public string OrderID { get; set; } = null!;

    public DateTime Date { get; set; }

    public int Qty { get; set; }

    public int Status { get; set; }

    public string? Memo { get; set; }

    public string EventID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public int PaymentID { get; set; }


    public virtual Event Event { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;


    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual Payment Payment { get; set; } = null!;
}
