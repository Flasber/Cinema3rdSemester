using System.Collections.Generic;
using BioProjektModels;

namespace BioProjekt.Api.Data.Mockdatabase
{
    public interface ICinemaRepository
    {
        IEnumerable<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        IEnumerable<Screening> GetAllScreenings();
        IEnumerable<Screening> GetScreeningsForMovie(int movieId);
    }
}
