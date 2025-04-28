using System;
using System.Collections.Generic;
using System.Linq;
using BioProjektModels;
using BioProjekt.Api.Data.Mockdatabase;

namespace BioProjekt.Api.Data.Mock
{
    public class MockCinemaRepository : ICinemaRepository
    {
        private readonly List<Movie> _movies;
        private readonly List<Screening> _screenings;

        public MockCinemaRepository()
        {
            _movies = new List<Movie>
{
    new Movie
    {
        Id = 1,
        Title = "Inception",
        Genre = "Sci-Fi",
        Duration = TimeSpan.FromMinutes(148),
        Description = "A mind-bending thriller about dreams within dreams.",
        Language = "English",
        AgeRating = "PG-13",
        PosterUrl = "https://m.media-amazon.com/images/I/81p+xe8cbnL._AC_SY679_.jpg"
    },
    new Movie
    {
        Id = 2,
        Title = "Mac and Devin Go To High School",
        Genre = "Comedy",
        Duration = TimeSpan.FromMinutes(75),
        Description = "Two students bond over cannabis and friendship.",
        Language = "English",
        AgeRating = "PG-18",
        PosterUrl = "https://upload.wikimedia.org/wikipedia/en/6/6e/Mac_and_Devin_Go_to_High_School_Poster.jpg"
    }
};


            _screenings = new List<Screening>
{
    new Screening
    {
        Id = 1,
        MovieId = 1,
        Date = DateTime.Today,
        Time = "18:00", 
        LanguageVersion = "English",
        Is3D = false,
        IsSoldOut = false,
        SoundSystem = "Dolby Atmos"
    },
    new Screening
    {
        Id = 2,
        MovieId = 2,
        Date = DateTime.Today.AddDays(1),
        Time = "20:00", 
        LanguageVersion = "English",
        Is3D = true,
        IsSoldOut = false,
        SoundSystem = "IMAX"
    }
};
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return _movies;
        }

        public Movie GetMovieById(int id)
        {
            return _movies.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Screening> GetAllScreenings()
        {
            return _screenings;
        }

        public IEnumerable<Screening> GetScreeningsForMovie(int movieId)
        {
            return _screenings.Where(s => s.MovieId == movieId);
        }
    }
}
