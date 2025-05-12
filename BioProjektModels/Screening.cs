namespace BioProjektModels
{
    public class Screening
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string LanguageVersion { get; set; }
        public bool Is3D { get; set; }
        public bool IsSoldOut { get; set; }
        public string SoundSystem { get; set; }
        public int MovieId { get; set; }
        public int AuditoriumId { get; set; }
        public DateTime StartDateTime => Date.Add(Time);

    }
}
