using BioProjektModels;
using Microsoft.AspNetCore.Mvc;

public class BookingController : Controller
{
    private readonly HttpClient _httpClient;

    public BookingController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // Action for selecting seats
    public async Task<IActionResult> SelectSeats(int showtimeId)
    {
        var seats = await _httpClient.GetFromJsonAsync<List<Seat>>($"http://localhost:5019/api/seats/available?auditoriumId={showtimeId}");
        return View(seats); // Display available seats for the selected showtime
    }

    // Action for confirming booking
    public async Task<IActionResult> ConfirmBooking(int seatId)
    {
        var result = await _httpClient.PostAsJsonAsync($"http://localhost:5019/api/seats/reserve", new { SeatId = seatId });

        if (result.IsSuccessStatusCode)
        {
            return RedirectToAction("BookingConfirmation");
        }

        return View("Error", new { message = "Booking failed" });
    }

    // Booking confirmation action
    public IActionResult BookingConfirmation()
    {
        return View();
    }
}
