using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Order
{
    [Display(Name = "訂單號碼")]
    public string OrderID { get; set; } = null!;

    [Display(Name = "訂單日期")]
    public DateTime Date { get; set; }

    [Display(Name = "總票數")]
    public int Qty { get; set; }

    public int Status { get; set; }

    [Display(Name = "備註")]
    public string? Memo { get; set; }

    public string EventID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    [Display(Name = "總金額")]
    public decimal TotalPrice { get; set; }

    [Display(Name = "演出名稱")]
    public virtual Event? Event { get; set; } = null!;

    [Display(Name = "會員姓名")]
    public virtual Member? Member { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

}
