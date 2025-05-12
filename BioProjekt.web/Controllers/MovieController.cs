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

        // Action for listing all movies
        public async Task<IActionResult> Index()
        {
            var movies = await _httpClient.GetFromJsonAsync<List<Movie>>("http://localhost:5019/api/Movie");
            return View(movies);
        }

        // Action for displaying showtimes of a specific movie
        public async Task<IActionResult> ShowTimes(int id)
        {
            var movie = await _httpClient.GetFromJsonAsync<Movie>($"http://localhost:5019/api/Movie/{id}");
            var showtimes = await _httpClient.GetFromJsonAsync<List<Screening>>($"http://localhost:5019/api/showtime/{id}");

            if (movie == null || showtimes == null)
            {
                return NotFound();
            }

            ViewData["MovieTitle"] = movie.Title;
            return View(showtimes); // Return showtimes to the ShowTimes view
        }

        // Action to show the seats for a specific screening (call API to get seats)
        public async Task<IActionResult> SelectSeats(int showtimeId)
        {
            var seats = await _httpClient.GetFromJsonAsync<List<Seat>>($"http://localhost:5019/api/seats/{showtimeId}");
            return View(seats); // Display seats for the selected screening
        }

        // Action to confirm booking (post API request to update the seat availability)
        public async Task<IActionResult> ConfirmBooking(int seatId)
        {
            var result = await _httpClient.PostAsJsonAsync($"http://localhost:5019/api/booking/{seatId}", new { SeatId = seatId });

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
