using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BioProjekt.web.Controllers
{
    public class MovieController : Controller
    {
        private readonly HttpClient _httpClient;

        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _httpClient.GetFromJsonAsync<List<Movie>>("http://localhost:5019/api/Movie");
            return View(movies);
        }
    }
}
