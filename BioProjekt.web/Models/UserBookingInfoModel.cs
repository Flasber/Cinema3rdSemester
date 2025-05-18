namespace BioProjekt.web.Models
{
    public class UserBookingInfoModel
    {
        public Guid SessionId { get; set; }
        public int ScreeningId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CustomerType { get; set; }
    }
}
