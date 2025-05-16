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
        ViewBag.ScreeningId = showtimeId;

        return View(seats);
    }

    public async Task<IActionResult> ConfirmBooking(int seatNumber, string row, string version, int auditoriumId, Guid sessionId, int screeningId)
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

        return RedirectToAction("BookingConfirmation", new { screeningId = screeningId, sessionId = sessionId });
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

    public IActionResult BookingForm(int screeningId)
    {
        return View(screeningId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWithoutLogin(
       int screeningId,
       string firstName,
       string lastName,
       string email,
       string confirmEmail,
       string phone,
       string address,
       string customerType)
    {
        if (email != confirmEmail)
            return View("Error", new ErrorViewModel { Message = "E-mailadresserne matcher ikke." });

        if (!Request.Cookies.TryGetValue("sessionId", out var sessionIdStr) || !Guid.TryParse(sessionIdStr, out var sessionId))
            return View("Error", new ErrorViewModel { Message = "Session ID mangler." });

        var dto = new BookingCustomerCreateDTO
        {
            ScreeningId = screeningId,
            Name = $"{firstName} {lastName}",
            Email = email,
            MobileNumber = phone,
            Address = address,
            CustomerType = customerType,
            SessionId = sessionId
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5019/api/booking/createWithCustomer", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Kunne ikke oprette booking. {error}" });
        }

        return RedirectToAction("BookingCompleted", new { screeningId, sessionId });
    }


    public async Task<IActionResult> BookingCompleted(int screeningId, Guid sessionId)
    {
        var screening = await _httpClient.GetFromJsonAsync<Screening>($"http://localhost:5019/api/screening/{screeningId}");
        var movie = await _httpClient.GetFromJsonAsync<Movie>($"http://localhost:5019/api/movie/{screening.MovieId}");
        var selectedSeats = await _httpClient.GetFromJsonAsync<List<Seat>>($"http://localhost:5019/api/seats/selection?sessionId={sessionId}");

        var seatLabels = selectedSeats.Select(s => $"{s.Row}{s.SeatNumber}").ToList();

        var viewModel = new BookingConfirmationViewModel
        {
            MovieTitle = movie.Title,
            PosterUrl = movie.PosterUrl ?? "/images/posters/default.jpg",
            AuditoriumName = $"Sal {screening.AuditoriumId}",
            StartTime = screening.StartDateTime,
            EndTime = screening.StartDateTime.AddMinutes(120),
            SeatLabels = seatLabels,
            TotalPrice = seatLabels.Count * 65,
            ScreeningId = screeningId
        };

        return View("BookingCompleted", viewModel);
    }
}
