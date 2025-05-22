namespace BioProjekt.Shared.WebDtos
{
    public class BookingCustomerCreateDTO
    {
        public int ScreeningId { get; set; }
        public Guid SessionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string CustomerType { get; set; } = "Standard";

        public List<int> ScreeningSeatIds { get; set; } = new();

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
