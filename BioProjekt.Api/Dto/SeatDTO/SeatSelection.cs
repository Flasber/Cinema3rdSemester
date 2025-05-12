namespace BioProjekt.Api.Dto.SeatDTO
{
    public class SeatSelectionDTO
    {
        public Guid SessionId { get; set; }
        public int SeatNumber { get; set; }
        public string Row { get; set; }
        public int AuditoriumId { get; set; }
    }

}
