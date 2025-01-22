using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class EventTag
{
    public string EventID { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
