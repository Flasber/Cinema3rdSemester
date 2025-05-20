namespace BioProjekt.Shared.ClientDtos
{
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public string PosterUrl { get; set; }
        public List<ScreeningCreateDto> Screenings { get; set; } = new();

    }
}
