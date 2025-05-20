namespace BioProjekt.Shared.WebDtos
{
    public class SeatSelectionDTO
    {
        public Guid SessionId { get; set; }
        public List<int> ScreeningSeatIds { get; set; } = new();
    }
}
