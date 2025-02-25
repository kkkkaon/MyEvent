using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

public partial class Credentials
{
    [Display(Name="帳號")]
    public string Account { get; set; } = null!;

    [Display(Name = "密碼")]
    public string Password { get; set; } = null!;

    public string? MemberID { get; set; }

    public virtual Member? Member { get; set; }
}
