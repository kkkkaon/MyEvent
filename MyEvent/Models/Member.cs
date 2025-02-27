using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Member
{
    
    public string MemberID { get; set; } = null!;

    [Display(Name = "姓名")]
    [Required(ErrorMessage = "此欄為必填")]
    public string MemberName { get; set; } = null!;

    [Display(Name = "生日")]
    [Required(ErrorMessage = "此欄為必填")]
    public DateOnly Birthday { get; set; }

    public DateTime JoinDate { get; set; }

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

    public string Role { get; set; } = null!;

    public virtual Credentials? Credentials { get; set; } = null!;
    public virtual RoleList? RoleList { get; set; } = null!;

    public virtual ICollection<CreditCard> CreditCard { get; set; } = new List<CreditCard>();
    public virtual ICollection<MemberTel> MemberTel { get; set; } = new List<MemberTel>();

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    internal void ParseTo<T>()
    {
        throw new NotImplementedException();
    }
}
