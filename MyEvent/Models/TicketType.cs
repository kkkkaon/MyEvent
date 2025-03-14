using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class TicketType
{
    public string EventID { get; set; } = null!;

    public string TicketTypeID { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
    public virtual TicketTypeList TicketTypeList { get; set; }
}
