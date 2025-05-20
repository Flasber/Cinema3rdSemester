namespace BioProjekt.Shared.WebDtos
{
    public class SeatAvailability
    {
        public int ScreeningSeatId { get; set; }
        public int SeatNumber { get; set; }
        public string Row { get; set; }
        public bool IsAvailable { get; set; }
        public byte[] Version { get; set; }
    }
}
