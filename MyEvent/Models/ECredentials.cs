using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models
{
    public class ECredentials
    {
        public string Account { get; set; }

        public string EventHolderID { get; set; }

        public string Password { get; set; }

        public virtual EventHolder? EventHolder { get; set; }

    }
}
