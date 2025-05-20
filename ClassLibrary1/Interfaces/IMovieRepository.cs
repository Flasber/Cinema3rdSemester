using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task CreateMovieAsync(Movie movie);

    }
}
