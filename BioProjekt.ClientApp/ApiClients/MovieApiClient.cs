using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IO;
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
                BaseAddress = new System.Uri("http://localhost:5019") // Ret hvis din API bruger en anden port
            };
        }

        public async Task<string?> UploadPosterAsync(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            using var fileStream = File.OpenRead(filePath);
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "poster", Path.GetFileName(filePath));

            var response = await _httpClient.PostAsync("/api/movie/upload-poster", content);
            if (!response.IsSuccessStatusCode)
                return null;

            var relativeUrl = await response.Content.ReadAsStringAsync();
            return _httpClient.BaseAddress + relativeUrl.Trim('"'); 
        }


        public async Task<bool> CreateMovieAsync(MovieCreateDto movie)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/movie", movie);
            return response.IsSuccessStatusCode;
        }
    }
}
