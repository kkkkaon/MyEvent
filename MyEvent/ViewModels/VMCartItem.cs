using System.ComponentModel.DataAnnotations;

namespace MyEvent.ViewModels
{
    public class VMCartItem
    {
        [Display(Name = "演出名稱")]
        [Required(ErrorMessage = "必填")]
        public string EventName { get; set; } = null!;

        [Display(Name = "演出日期")]
        [Required(ErrorMessage = "必填")]
        public DateOnly Date { get; set; }

        [Display(Name = "開始時間")]
        [Required(ErrorMessage = "必填")]
        public TimeOnly StartTime { get; set; }

        [Display(Name = "場館")]
        [Required(ErrorMessage = "必填")]
        public string VenueID { get; set; } = null!;

        //Seat
        public string SeatType { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Row { get; set; }
        public string? Number { get; set; }

        //TicketTypeList
        public string Name { get; set; } = null!;
        public double Discount { get; set; }
    }
}
