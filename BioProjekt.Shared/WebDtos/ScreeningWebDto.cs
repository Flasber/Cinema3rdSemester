namespace BioProjekt.Shared.WebDtos
{
    public class ScreeningWebDto
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MovieId { get; set; }
        public int AuditoriumId { get; set; }
    }
}
