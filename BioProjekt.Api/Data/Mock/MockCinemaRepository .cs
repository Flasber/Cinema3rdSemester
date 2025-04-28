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
        PosterUrl = "/images/Inception.jpg"
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
        PosterUrl = "/images/MacAndDevin.jpg"
    },
    new Movie
    {
        Id = 3,
        Title = "Minecraft Filmen",
        Genre = "Adventure",
        Duration = TimeSpan.FromMinutes(100),
        Description = "Et eventyr i en blokverden, hvor alt er muligt!",
        Language = "English",
        AgeRating = "PG",
        PosterUrl = "/images/Minecraft.jpg"
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
