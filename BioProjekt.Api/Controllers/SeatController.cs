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

        [HttpPost("select")]
        public async Task<IActionResult> SelectSeats([FromBody] SeatSelectionDTO dto)
        {
            var success = await _seatService.SelectSeats(dto.SessionId, dto.ScreeningSeatIds);

            if (!success)
                return BadRequest("En eller flere sæder kunne ikke vælges.");

            return Ok();
        }


        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeat([FromBody] SeatReservationRequestDTO dto)
        {
            var result = await _seatService.TryReserveScreeningSeat(dto.ScreeningSeatId, dto.ClientVersion);

            if (!result)
                return BadRequest("Sædet kunne ikke reserveres. Det er muligvis optaget.");

            return Ok();
        }

        [HttpGet("selection")]
        public async Task<IActionResult> GetSelectedSeats([FromQuery] Guid sessionId)
        {
            var selected = _seatService.GetSelectedSeats(sessionId);
            var output = selected.Select(ss => ss.Seat).ToList();
            return Ok(output);
        }
    }
}
