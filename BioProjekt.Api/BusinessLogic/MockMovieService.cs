using System;
using System.Collections.Generic;
using BioProjektModels;

namespace BioProjekt.Api.BusinessLogic
{
    public class MockMovieService : IMovieService
    {
        private static readonly List<Movie> Movies = new()
        {
            new Movie
            {
                Id = 1,
                Title = "A Minecraft Movie",
                Genre = "Fantasy Adventure Comedy",
                Duration = TimeSpan.FromMinutes(139),
                Description = "Creeper go BYE BYE",
                Language = "English",
                AgeRating = "PG-7"
            },
            new Movie
            {
                Id = 2,
                Title = "Until Dawn",
                Genre = "Horror",
                Duration = TimeSpan.FromHours(143),
                Description = "Can you stay alive.....",
                Language = "English",
                AgeRating = "PG-18"
            }
        };

        public List<Movie> GetAllMovies()
        {
            return Movies;
        }
    }
}
