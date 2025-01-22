using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class CreditCard
{
    public string CardNo { get; set; } = null!;

    public string ValidYear { get; set; } = null!;

    public string ValidMonth { get; set; } = null!;

    public string SecurityNo { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
