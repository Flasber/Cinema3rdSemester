using BioProjektModels;
using System;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        string GetMovieGenre(int id);
        string GetMovieDescription(int id);
        TimeSpan GetMovieDuration(int id);
    }
}
