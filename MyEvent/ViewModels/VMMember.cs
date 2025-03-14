using MyEvent.DTOs;
using MyEvent.Models;

namespace MyEvent.ViewModels
{
    public class VMMember
    {
        public Member? Member { get; set; }
        public Credentials? Credentials { get; set; }

        public CreditCard? CreditCard { get; set; }
    }
}
