using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Linq;
using BioProjekt.Api.Data.Mock;
using BioProjekt.Api.Data.Mockdatabase;

namespace BioProjekt.Api.BusinessLogic
{
    public class MovieService : IMovieService
    {
        private readonly ICinemaRepository _cinemaRepository;

        public MovieService(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public List<Movie> GetAllMovies()
        {
            return _cinemaRepository.GetAllMovies().ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _cinemaRepository.GetAllMovies().FirstOrDefault(m => m.Id == id);
        }

        public List<Movie> GetMoviesByGenre(string genre)
        {
            return _cinemaRepository.GetAllMovies()
                .Where(m => m.Genre != null && m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public string GetMovieDescription(int id)
        {
            var movie = GetMovieById(id);
            return movie?.Description;
        }

        public TimeSpan GetMovieDuration(int id)
        {
            var movie = GetMovieById(id);
            return movie?.Duration ?? TimeSpan.Zero;
        }
    }
}
