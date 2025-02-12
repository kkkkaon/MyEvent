using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Event
{
    
    public string EventID { get; set; } = null!;
    
    [Required(ErrorMessage = "必填")]
    public string EventName { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "必填")]
    public string VenueID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string EventHolderID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string Pic { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string Price { get; set; } = null!;

    public string? Discount { get; set; }

    public string? Note { get; set; }

    [Required(ErrorMessage = "必填")]
    public string EventTypeID { get; set; } = null!;

    public virtual EventHolder? EventHolder { get; set; }

    public virtual EventType EventType { get; set; } = null!;
    public virtual Venue Venue { get; set; } = null!;

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<EventTag> EventTag{ get; set; } = new List<EventTag>();
    public virtual ICollection<TicketType> TicketType { get; set; } = new List<TicketType>();

}
