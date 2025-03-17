using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Event
{
    [Display(Name = "演出編號")]
    public string EventID { get; set; } = null!;

    [Display(Name = "演出名稱")]
    [Required(ErrorMessage = "必填")]
    public string EventName { get; set; } = null!;

    [Display(Name = "演出日期")]
    public DateOnly Date { get; set; }

    [Display(Name = "開演時間")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "必填")]
    public string VenueID { get; set; } = null!;

    [Required(ErrorMessage = "必填")]
    public string EventHolderID { get; set; } = null!;

    [Display(Name = "節目介紹")]
    [Required(ErrorMessage = "必填")]
    public string Description { get; set; } = null!;

    [Display(Name = "圖示")]
    [Required(ErrorMessage = "必填")]
    public string Pic { get; set; } = null!;

    [Display(Name = "票價")]
    [Required(ErrorMessage = "必填")]
    public string Price { get; set; } = null!;

    [Display(Name = "折扣方案")]
    public string? Discount { get; set; }

    [Display(Name = "重要須知")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "必填")]
    public string EventTypeID { get; set; } = null!;

    public virtual EventHolder? EventHolder { get; set; }

    [Display(Name = "演出類型")]
    public virtual EventType? EventType { get; set; } = null!;

    [Display(Name = "場館")]
    public virtual Venue? Venue { get; set; } = null!;

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<EventTag> EventTag{ get; set; } = new List<EventTag>();

    public virtual ICollection<TicketType> TicketType { get; set; } = new List<TicketType>();

}
