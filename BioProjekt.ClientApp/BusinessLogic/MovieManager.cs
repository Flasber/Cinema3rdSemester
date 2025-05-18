using System.Threading.Tasks;
using BioProjekt.ClientApp.Forms.Services;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.ClientApp.BusinessLogic
{
    public class MovieManager
    {
        private readonly MovieApiClient _apiClient;

        public MovieManager()
        {
            _apiClient = new MovieApiClient();
        }

        public Task<bool> CreateMovieAsync(MovieCreateDto movie)
        {
            return _apiClient.CreateMovieAsync(movie);
        }
    }
}
