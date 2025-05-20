using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BioProjekt.Shared.ClientDtos;
namespace BioProjekt.ClientApp.Forms.Services
{
    public class MovieApiClient
    {
        private readonly HttpClient _httpClient;

        public MovieApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:5019")
            };
        }

        public async Task<bool> CreateMovieAsync(MovieCreateDto movie)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/movie", movie);
            return response.IsSuccessStatusCode;
        }
    }
}
