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

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSeats([FromQuery] int screeningId)
        {
            var seats = await _seatService.GetAvailableSeats(screeningId);
            return Ok(seats);
        }


        [HttpGet("selection")]
        public async Task<IActionResult> GetSelectedSeats([FromQuery] Guid sessionId)
        {
            var selected = _seatService.GetSelectedSeats(sessionId);
            var output = selected.Select(ss => ss.Seat).ToList();
            return Ok(output);
        }
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
