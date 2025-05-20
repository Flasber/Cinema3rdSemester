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
        private readonly string _apiBaseUrl;

        public MovieController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = config["ApiBaseUrl"];
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _httpClient.GetFromJsonAsync<List<Movie>>($"{_apiBaseUrl}/api/Movie");
            return View(movies);
        }

        public async Task<IActionResult> ShowTimes(int id)
        {
            var movie = await _httpClient.GetFromJsonAsync<Movie>($"{_apiBaseUrl}/api/Movie/{id}");
            var showtimes = await _httpClient.GetFromJsonAsync<List<Screening>>($"{_apiBaseUrl}/api/showtime/{id}");

            if (movie == null || showtimes == null)
            {
                return NotFound();
            }

            ViewData["MovieTitle"] = movie.Title;
            return View(showtimes);
        }

        public async Task<IActionResult> SelectSeats(int showtimeId)
        {
            var seats = await _httpClient.GetFromJsonAsync<List<Seat>>($"{_apiBaseUrl}/api/seats/{showtimeId}");
            return View(seats);
        }

        public async Task<IActionResult> ConfirmBooking(int seatId)
        {
            var result = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/booking/{seatId}", new { SeatId = seatId });

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("BookingConfirmation");
            }
            else
            {
                return View("Error", new { message = "Booking failed" });
            }
        }
    }
}
