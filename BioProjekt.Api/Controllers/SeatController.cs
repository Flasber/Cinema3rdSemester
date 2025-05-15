using Microsoft.AspNetCore.Mvc;
using BioProjekt.Api.BusinessLogic;
using BioProjektModels;
using BioProjekt.Shared.WebDtos;
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

            foreach (var seat in availableSeats)
            {
                Console.WriteLine($"Seat {seat.SeatNumber}: Version = {(seat.Version == null ? "NULL" : BitConverter.ToString(seat.Version))}");
            }

            return Ok(availableSeats);
        }


        [HttpPost("select")]
        public IActionResult SelectSeat([FromBody] SeatSelectionDTO selection)
        {
            Console.WriteLine($"==> API: SELECT seat {selection.Row}{selection.SeatNumber} for session {selection.SessionId}");

            var success = _seatService.SelectSeat(selection.SessionId, selection.SeatNumber, selection.Row, selection.AuditoriumId);

            if (!success)
            {
                Console.WriteLine("!! Sædevalg fejlede - sædet er ikke ledigt");
                return Conflict("Sædet er allerede reserveret.");
            }

            Console.WriteLine("==> Sædevalg gennemført og gemt");
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
