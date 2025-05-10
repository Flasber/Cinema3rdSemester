using BioProjektModels;
using BioProjektModels.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class MovieService : IMovieService
    {
        private readonly ISqlCinemaRepository _repository;

        public MovieService(ISqlCinemaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _repository.GetAllMoviesAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _repository.GetMovieByIdAsync(id);
        }
    }
}
