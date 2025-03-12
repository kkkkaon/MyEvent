using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models
{
    public class ECredentials
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "此欄為必填")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "此欄為必填")]
        public string Password { get; set; }

        public string? EventHolderID { get; set; }

        public virtual EventHolder? EventHolder { get; set; }

    }
}
