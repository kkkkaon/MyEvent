using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class MemberTel
{
    public string MemberID { get; set; } = null!;

    public string Tel { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
