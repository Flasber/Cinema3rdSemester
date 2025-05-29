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
    private readonly string _apiBaseUrl;

    public BookingController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = config["ApiBaseUrl"];
    }
    // GET: /Booking/SelectSeats?showtimeId=?
    // Viser sædeoversigten for en bestemt forestilling (screening) og opretter en ny session (cookie) for midlertidig reservation
    [HttpGet]
    public async Task<IActionResult> SelectSeats(int showtimeId)
    {
        var screening = await _httpClient.GetFromJsonAsync<ScreeningWebDto>($"{_apiBaseUrl}/api/screening/{showtimeId}");
        if (screening == null)
            return View("Error", new ErrorViewModel { Message = "Screening ikke fundet" });

        var seats = await _httpClient.GetFromJsonAsync<List<SeatAvailability>>(
            $"{_apiBaseUrl}/api/seats/available?screeningId={screening.Id}");

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

    // POST: /Booking/SelectSeats
    // Forsøger at reservere valgte sæder midlertidigt via API’et, og omdirigerer til bekræftelsessiden
    [HttpPost]
    public async Task<IActionResult> SelectSeats([FromForm] List<int> selectedSeatIds, [FromForm] Guid sessionId, [FromForm] int screeningId)
    {
        var dto = new SeatSelectionDTO
        {
            SessionId = sessionId,
            ScreeningSeatIds = selectedSeatIds
        };

        var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/seats/select", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return View("Error", new ErrorViewModel { Message = $"Sædevalg fejlede: {error}" });
        }

        return RedirectToAction("BookingConfirmation", new { screeningId, sessionId });
    }

    // GET: /Booking/BookingConfirmation
    // Viser en opsummering af film, tid, valgte sæder og totalpris, inden bookingen bekræftes
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

        var screening = await _httpClient.GetFromJsonAsync<ScreeningWebDto>($"{_apiBaseUrl}/api/screening/{screeningId}");
        var movie = await _httpClient.GetFromJsonAsync<Movie>($"{_apiBaseUrl}/api/movie/{screening.MovieId}");
        var selectedSeats = await _httpClient.GetFromJsonAsync<List<SeatAvailability>>(
            $"{_apiBaseUrl}/api/seats/selection?sessionId={sessionId}");

        var seatLabels = selectedSeats.Select(s => $"{s.Row}{s.SeatNumber}").ToList();
        var totalPrice = seatLabels.Count * 65;

        var viewModel = new BookingConfirmationViewModel
        {
            MovieTitle = movie.Title,
            PosterUrl = movie.PosterUrl ?? "/images/posters/default.jpg",
            AuditoriumName = $"Sal {screening.AuditoriumId}",
            StartTime = screening.StartDateTime,
            EndTime = screening.EndDateTime,
            SeatLabels = seatLabels,
            TotalPrice = totalPrice,
            ScreeningId = screening.Id
        };

        return View(viewModel);
    }

    // POST: /Booking/BookingConfirmation
    // Afsender kundeoplysninger og valgte sæder til API’et, opretter en booking i databasen
    // og viser en kvitteringsside med bookingoplysninger, hvis alt lykkes
    [HttpPost]
    public async Task<IActionResult> BookingConfirmation(UserBookingInfoModel model)
    {
        if (!ModelState.IsValid || model.Email != model.ConfirmEmail)
        {
            return View("Error", new ErrorViewModel { Message = "Ugyldige oplysninger eller e-mails matcher ikke." });
        }

        var selectedSeats = await _httpClient.GetFromJsonAsync<List<SeatAvailability>>(
            $"{_apiBaseUrl}/api/seats/selection?sessionId={model.SessionId}");

        if (selectedSeats == null || selectedSeats.Count == 0)
        {
            return View("Error", new ErrorViewModel { Message = "Ingen sæder valgt for denne session." });
        }

        var screening = await _httpClient.GetFromJsonAsync<ScreeningWebDto>(
            $"{_apiBaseUrl}/api/screening/{model.ScreeningId}");

        var movie = await _httpClient.GetFromJsonAsync<Movie>($"{_apiBaseUrl}/api/movie/{screening.MovieId}");

        var dto = new BookingCustomerCreateDTO
        {
            ScreeningId = model.ScreeningId,
            SessionId = model.SessionId,
            Name = model.FirstName + " " + model.LastName,
            Email = model.Email,
            MobileNumber = model.Phone,
            Address = model.Address,
            CustomerType = model.CustomerType,
            ScreeningSeatIds = selectedSeats.Select(s => s.ScreeningSeatId).ToList(),
            StartTime = screening.StartDateTime,
            EndTime = screening.EndDateTime
        };

        var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/api/booking/createWithCustomer", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            if (error.Contains("allerede taget", StringComparison.OrdinalIgnoreCase))
            {
                TempData["BookingError"] = "Et eller flere af de valgte sæder er desværre allerede reserveret. Vælg venligst nye sæder.";
                return RedirectToAction("SelectSeats", new { showtimeId = model.ScreeningId });
            }

            return View("Error", new ErrorViewModel { Message = $"Booking fejlede: {error}" });
        }

        var viewModel = new BookingConfirmationViewModel
        {
            MovieTitle = movie.Title,
            PosterUrl = movie.PosterUrl ?? "/images/posters/default.jpg",
            AuditoriumName = $"Sal {screening.AuditoriumId}",
            StartTime = screening.StartDateTime,
            EndTime = screening.EndDateTime,
            SeatLabels = selectedSeats.Select(s => $"{s.Row}{s.SeatNumber}").ToList(),
            TotalPrice = selectedSeats.Count * 65,
            ScreeningId = screening.Id
        };

        return View("BookingCompleted", viewModel);
    }
}
