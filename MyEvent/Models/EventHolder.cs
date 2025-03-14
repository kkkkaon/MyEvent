using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class EventHolder
{
    public string EventHolderID { get; set; } = null!;

    [Display(Name = "主辦團體")]
    [Required(ErrorMessage = "此欄為必填")]
    public string EventHolderName { get; set; } = null!;

    [Display(Name = "加入日期")]
    public DateTime JoinDate { get; set; }

    [Display(Name = "電話")]
    [Required(ErrorMessage = "此欄為必填")]
    public string Tel { get; set; } = null!;

    [Display(Name = "郵遞區號")]
    [Required(ErrorMessage = "此欄為必填")]
    public string ZipCode { get; set; } = null!;

    [Display(Name = "城市")]
    [Required(ErrorMessage = "此欄為必填")]
    public string City { get; set; } = null!;

    [Display(Name = "區域")]
    [Required(ErrorMessage = "此欄為必填")]
    public string Area { get; set; } = null!;

    [Display(Name = "地址")]
    [Required(ErrorMessage = "此欄為必填")]
    public string Address { get; set; } = null!;

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();

    public virtual ECredentials? ECredentials { get; set; } = null!;
}
