namespace BioProjekt.Api.Dto.SeatDTO
{
    public class SeatAvailability
    {
        public int SeatNumber { get; set; }
        public string Row { get; set; }
        public bool IsAvailable { get; set; }
    }
}
