using BioProjekt.Api.BusinessLogic;
using BioProjekt.Shared.WebDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }
        // GET: /api/seats/available?screeningId=123
        // Returnerer ledige sæder for en bestemt forestilling
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSeats([FromQuery] int screeningId)
        {
            var seats = await _seatService.GetAvailableSeats(screeningId);
            return Ok(seats);
        }

        // GET: /api/seats/selection?sessionId=guid
        // Returnerer de sæder, som brugeren har valgt i sin session
        [HttpGet("selection")]
        public async Task<IActionResult> GetSelectedSeats([FromQuery] Guid sessionId)
        {
            var selected = _seatService.GetSelectedSeats(sessionId);
            var output = selected.Select(ss => ss.Seat).ToList();
            return Ok(output);
        }
        // POST: /api/seats/select
// Forsøger at reservere de valgte sæder midlertidigt til en session
        [HttpPost("select")]
        public async Task<IActionResult> SelectSeats([FromBody] SeatSelectionDTO dto)
        {
            try
            {
                var success = await _seatService.SelectSeats(dto.SessionId, dto.ScreeningSeatIds);

                if (!success)
                    return BadRequest("Et eller flere sæder kunne ikke vælges. De kan være reserveret.");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Der opstod en uventet fejl. Prøv igen senere. {ex.Message}");
            }
        }

    }
}
