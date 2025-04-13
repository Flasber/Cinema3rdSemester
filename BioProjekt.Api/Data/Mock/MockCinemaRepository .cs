using System;
using System.Collections.Generic;
using System.Linq;

namespace BioProjekt.Api.Data.Mockdatabase
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public DateTime DateOfPlaying { get; set; }
        public string AgeRating { get; set; }
    }

    public class Screening
    {
        public int ScreeningID { get; set; }
        public DateTime Time { get; set; }
        public int MovieID { get; set; }
        public bool IsSoldOut { get; set; }
    }

    public class Seat
    {
        public string SeatNumber { get; set; }
        public string Row { get; set; }
        public string SeatType { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class MockTicketService
    {
        private readonly List<Movie> _movies;
        private readonly List<Screening> _screenings;
        private readonly List<Seat> _seats;

        public MockTicketService()
        {
            _movies = new List<Movie>
            {
                new Movie { MovieID = 1, Title = "Inception", Language = "English", DateOfPlaying = DateTime.Today, AgeRating = "PG-13" },
                new Movie { MovieID = 2, Title = "Mac and Devin Goes To HighSchool", Language = "English", DateOfPlaying = DateTime.Today.AddDays(1), AgeRating = "PG-18" }
            };

            _screenings = new List<Screening>
            {
                new Screening { ScreeningID = 1, MovieID = 1, Time = DateTime.Today.AddHours(18), IsSoldOut = false },
                new Screening { ScreeningID = 2, MovieID = 2, Time = DateTime.Today.AddDays(1).AddHours(20), IsSoldOut = false }
            };

            _seats = new List<Seat>
            {
                new Seat { SeatNumber = "A1", Row = "A", SeatType = "Standard", IsAvailable = true },
                new Seat { SeatNumber = "A2", Row = "A", SeatType = "VIP", IsAvailable = false }
            };
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movies;
        }

        public IEnumerable<Screening> GetScreeningsForMovie(int movieId)
        {
            return _screenings.Where(s => s.MovieID == movieId);
        }

        public IEnumerable<Seat> GetAvailableSeats()
        {
            return _seats.Where(s => s.IsAvailable);
        }
    }
}
