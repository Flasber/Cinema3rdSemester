using BioProjektModels;
using System;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        List<Movie> GetMoviesByGenre(string genre);
        string GetMovieDescription(int id);
        TimeSpan GetMovieDuration(int id);
    }
}
