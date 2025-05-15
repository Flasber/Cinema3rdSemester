using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
    }
}
