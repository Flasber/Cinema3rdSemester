namespace BioProjektModels
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int ScreeningId { get; set; }
        public DateTime BookingDate { get; set; }
        public int CustomerNumber { get; set; }
        public string BookingStatus { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscounted { get; set; }

        public Customer Customer { get; set; }
    }
}
