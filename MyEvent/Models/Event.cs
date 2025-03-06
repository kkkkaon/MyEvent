using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Event
{
    public string EventID { get; set; } = null!;

    [Display(Name = "演出名稱")]
    [Required(ErrorMessage = "必填")]
    public string EventName { get; set; } = null!;

    [Display(Name = "演出日期")]
    [Required(ErrorMessage = "必填")]
    public DateOnly Date { get; set; }

    [Display(Name = "開始時間")]
    [Required(ErrorMessage = "必填")]
    public TimeOnly StartTime { get; set; }

    [Display(Name = "場館")]
    [Required(ErrorMessage = "必填")]
    public string VenueID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string EventHolderID { get; set; } = null!;

    [Display(Name = "演出內容")]
    [Required(ErrorMessage = "必填")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = null!;

    [Display(Name = "圖示")]
    [Required(ErrorMessage = "必填")]
    public string Pic { get; set; } = null!;

    [Display(Name = "票價")]
    [Required(ErrorMessage = "必填")]
    public string Price { get; set; } = null!;

    [Display(Name = "折扣方案")]
    [DataType(DataType.MultilineText)]
    public string? Discount { get; set; }

    [Display(Name = "重要須知")]
    [DataType(DataType.MultilineText)]
    public string? Note { get; set; }

    [Display(Name = "演出類型")]
    [Required(ErrorMessage = "必填")]
    public string EventTypeID { get; set; } = null!;

    public virtual EventHolder? EventHolder { get; set; }

    public virtual EventType? EventType { get; set; } = null!;
    public virtual Venue? Venue { get; set; } = null!;

    public virtual ICollection<Order>? Order { get; set; } = new List<Order>();

    public virtual ICollection<EventTag>? EventTag{ get; set; } = new List<EventTag>();
    public virtual ICollection<TicketType>? TicketType { get; set; } = new List<TicketType>();

}
