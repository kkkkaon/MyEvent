using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class EventType
{
    [Display(Name = "類型")]
    [Required(ErrorMessage = "必填")]
    public string EventTypeID { get; set; } = null!;

    [Display(Name = "類型")]
    [Required(ErrorMessage = "必填")]
    public string EventType1 { get; set; } = null!;

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();
}
