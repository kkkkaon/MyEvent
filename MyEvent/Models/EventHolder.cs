using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class EventHolder
{
    public string EventHolderID { get; set; } = null!;

    public string EventHolderName { get; set; } = null!;

    public DateTime JoinDate { get; set; }

    public string Tel { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();
}
