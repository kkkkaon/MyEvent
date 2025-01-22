using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Member
{
    public string MemberID { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public DateTime JoinDate { get; set; }

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Credentials> Credentials { get; set; } = new List<Credentials>();

    public virtual ICollection<CreditCard> CreditCard { get; set; } = new List<CreditCard>();

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}
