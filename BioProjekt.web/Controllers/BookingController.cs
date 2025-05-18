using BioProjektModels;
using Microsoft.AspNetCore.Mvc;
using BioProjekt.web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioProjekt.Shared.WebDtos;
using System;

public class BookingController : Controller
{
    private readonly HttpClient _httpClient;

    public BookingController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> SelectSeats(int showtimeId)
    {
        var screening = await _httpClient.GetFromJsonAsync<Screening>($"http://localhost:5019/api/screening/{showtimeId}");
        if (screening == null)
            return View("Error", new ErrorViewModel { Message = "Screening ikke fundet" });

        var seats = await _httpClient.GetFromJsonAsync<List<SeatAvailability>>(
            $"http://localhost:5019/api/seats/available?screeningId={screening.Id}");

        var sessionId = Guid.NewGuid();
        Response.Cookies.Append("sessionId", sessionId.ToString(), new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Secure = true
        });

        ViewBag.SessionId = sessionId;
        ViewBag.ScreeningId = showtimeId;

        return View(seats);
    }

    [HttpPost]
    public async Task<IActionResult> SelectSeats([FromForm] List<int> selectedSeatIds, [FromForm] Guid sessionId, [FromForm] int screeningId)
    {
        var dto = new SeatSelectionDTO
        {
            SessionId = sessionId,
            ScreeningSeatIds = selectedSeatIds
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/seats/select", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Sædevalg fejlede: {error}" });
        }

        return RedirectToAction("BookingConfirmation", new { screeningId, sessionId });
    }

    [HttpGet]
    public async Task<IActionResult> BookingConfirmation(int screeningId, Guid? sessionId)
    {
        if (!sessionId.HasValue)
        {
            if (!Request.Cookies.TryGetValue("sessionId", out var sessionIdStr) || !Guid.TryParse(sessionIdStr, out var parsedId))
            {
                return View("Error", new ErrorViewModel { Message = "Session ID mangler. Prøv at vælge sæder igen." });
            }
            sessionId = parsedId;
        }

        var screening = await _httpClient.GetFromJsonAsync<Screening>($"http://localhost:5019/api/screening/{screeningId}");
        var movie = await _httpClient.GetFromJsonAsync<Movie>($"http://localhost:5019/api/movie/{screening.MovieId}");
        var selectedSeats = await _httpClient.GetFromJsonAsync<List<Seat>>($"http://localhost:5019/api/seats/selection?sessionId={sessionId}");

        var seatLabels = selectedSeats.Select(s => $"{s.Row}{s.SeatNumber}").ToList();
        var totalPrice = seatLabels.Count * 65;

        var viewModel = new BookingConfirmationViewModel
        {
            MovieTitle = movie.Title,
            PosterUrl = movie.PosterUrl ?? "/images/posters/default.jpg",
            AuditoriumName = $"Sal {screening.AuditoriumId}",
            StartTime = screening.StartDateTime,
            EndTime = screening.StartDateTime.AddMinutes(120),
            SeatLabels = seatLabels,
            TotalPrice = totalPrice,
            ScreeningId = screening.Id
        };

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> BookingConfirmation(UserBookingInfoModel model)
    {
        if (!ModelState.IsValid || model.Email != model.ConfirmEmail)
        {
            return View("Error", new ErrorViewModel { Message = "Ugyldige oplysninger eller e-mails matcher ikke." });
        }

        var dto = new BookingCustomerCreateDTO
        {
            ScreeningId = model.ScreeningId,
            SessionId = model.SessionId,
            Name = model.FirstName + " " + model.LastName,
            Email = model.Email,
            MobileNumber = model.Phone,
            Address = model.Address,
            CustomerType = model.CustomerType
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/booking/createWithCustomer", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Booking fejlede: {error}" });
        }

        return RedirectToAction("Completed");
    }

    [HttpGet]
    public IActionResult Completed()
    {
        return View("BookingCompleted"); 
    }

}
