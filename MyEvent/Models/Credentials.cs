using System;
using System.Collections.Generic;

namespace MyEvent.Models;

public partial class Credentials
{
    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
