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

    public async Task<IActionResult> SelectSeats(int showtimeId)
    {
        var screening = await _httpClient.GetFromJsonAsync<Screening>($"http://localhost:5019/api/screening/{showtimeId}");
        if (screening == null)
            return View("Error", new ErrorViewModel { Message = "Screening ikke fundet" });

        var seats = await _httpClient.GetFromJsonAsync<List<SeatAvailability>>(
            $"http://localhost:5019/api/seats/available?auditoriumId={screening.AuditoriumId}");

        var sessionId = Guid.NewGuid();
        Response.Cookies.Append("sessionId", sessionId.ToString(), new CookieOptions
        {
            SameSite = SameSiteMode.Lax,
            Secure = true
        });

        ViewBag.SessionId = sessionId;

        return View(seats);
    }

    public async Task<IActionResult> ConfirmBooking(int seatNumber, string row, string version, int auditoriumId, Guid sessionId)
    {
        var selectDto = new SeatSelectionDTO
        {
            SessionId = sessionId,
            SeatNumber = seatNumber,
            Row = row,
            AuditoriumId = auditoriumId
        };

        var selectResult = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/seats/select", selectDto);
        if (!selectResult.IsSuccessStatusCode)
        {
            var error = await selectResult.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Sædevalg fejlede: {error}" });
        }

        var dto = new SeatReservationRequestDTO
        {
            SeatNumber = seatNumber,
            Row = row,
            ClientVersion = Convert.FromBase64String(version),
            AuditoriumId = auditoriumId
        };

        var reserveResult = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/seats/reserve", dto);
        if (!reserveResult.IsSuccessStatusCode)
        {
            var error = await reserveResult.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Sædereservation fejlede: {error}" });
        }

        return RedirectToAction("BookingConfirmation", new { screeningId = 1, sessionId = sessionId });
    }

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

        var screeningResponse = await _httpClient.GetAsync($"http://localhost:5019/api/screening/{screeningId}");
        if (!screeningResponse.IsSuccessStatusCode)
        {
            var error = await screeningResponse.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Screening ikke fundet. Server sagde: {error}" });
        }

        var screening = await screeningResponse.Content.ReadFromJsonAsync<Screening>();

        var movieResponse = await _httpClient.GetAsync($"http://localhost:5019/api/movie/{screening.MovieId}");
        if (!movieResponse.IsSuccessStatusCode)
        {
            var error = await movieResponse.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Film ikke fundet. Server sagde: {error}" });
        }

        var movie = await movieResponse.Content.ReadFromJsonAsync<Movie>();

        var seatResponse = await _httpClient.GetAsync($"http://localhost:5019/api/seats/selection?sessionId={sessionId}");
        if (!seatResponse.IsSuccessStatusCode)
        {
            var error = await seatResponse.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Valgte sæder ikke fundet. Server sagde: {error}" });
        }

        var selectedSeats = await seatResponse.Content.ReadFromJsonAsync<List<Seat>>();
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
            TotalPrice = totalPrice
        };

        return View(viewModel);
    }

    public IActionResult BookingForm(int screeningId)
    {
        return View(screeningId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWithoutLogin(int screeningId)
    {
        var dto = new BookingCreateDTO
        {
            CustomerNumber = 1,
            ScreeningId = screeningId
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/booking", dto);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("BookingConfirmation", new { screeningId = screeningId });

        return View("Error", new ErrorViewModel { Message = "Kunne ikke oprette booking." });
    }
}
