using Microsoft.AspNetCore.Mvc;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Dto.SeatDTO;
using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/seats")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeat([FromBody] SeatReservationRequestDTO request)
        {
            var success = await _seatService.TryReserveSeat(
                request.SeatNumber,
                request.Row,
                request.ClientVersion,
                request.AuditoriumId);

            if (!success)
                return Conflict("Sædet er allerede reserveret eller versionen er forkert.");

            return Ok("Reservation gennemført.");
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<SeatAvailability>>> GetAvailableSeats([FromQuery] int auditoriumId)
        {
            var availableSeats = await _seatService.GetAvailableSeats(auditoriumId);

            if (!availableSeats.Any())
                return NotFound("Ingen ledige sæder fundet.");

            return Ok(availableSeats);
        }

        [HttpPost("select")]
        public IActionResult SelectSeat([FromBody] SeatSelectionDTO selection)
        {
            var success = _seatService.SelectSeat(selection.SessionId, selection.SeatNumber, selection.Row, selection.AuditoriumId);

            if (!success)
                return Conflict("Sædet er allerede reserveret.");

            return Ok("Sædevalg gemt midlertidigt.");
        }

        [HttpGet("selection")]
        public ActionResult<IEnumerable<Seat>> GetSelectedSeats([FromQuery] Guid sessionId)
        {
            var selectedSeats = _seatService.GetSelectedSeats(sessionId);

            if (!selectedSeats.Any())
                return NotFound("Ingen sæder valgt for denne session.");

            return Ok(selectedSeats);
        }
    }
}
