﻿namespace BioProjekt.web.Models
{
    public class BookingConfirmationViewModel
    {
        public string MovieTitle { get; set; }
        public string PosterUrl { get; set; }
        public string AuditoriumName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string> SeatLabels { get; set; } 
        public decimal TotalPrice { get; set; }
        public int ScreeningId { get; set; }
    }
}
