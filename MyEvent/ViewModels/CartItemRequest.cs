namespace MyEvent.ViewModels
{
    public class CartItemRequest
    {
        public string EventID { get; set; }
        public string SeatID { get; set; }
        public string TicketTypeID { get; set; }
        public int Quantity { get; set; }
    }
}
