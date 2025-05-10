using Microsoft.AspNetCore.Mvc;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Dto.SeatDTO;
using BioProjektModels;

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
        public IActionResult ReserveSeat([FromBody] SeatReservationRequestDTO request)
        {
            var success = _seatService.TryReserveSeat(
                request.SeatNumber,
                request.Row,
                request.ClientVersion,
                request.AuditoriumId);

            if (!success)
                return Conflict("Sædet er allerede reserveret eller versionen er forkert.");

            return Ok("Reservation gennemført.");
        }

        [HttpGet("available")]
        public ActionResult<IEnumerable<SeatAvailability>> GetAvailableSeats([FromQuery] int auditoriumId)
        {
            var availableSeats = _seatService.GetAvailableSeats(auditoriumId);

            if (!availableSeats.Any())
                return NotFound("Ingen ledige sæder fundet.");

            return Ok(availableSeats);
        }

        [HttpPost("select")]
        public IActionResult SelectSeat([FromBody] SeatSelectionDTO selection)
        {
            var success = _seatService.SelectSeatForBooking(selection.BookingId, selection.SeatNumber, selection.Row, selection.AuditoriumId);

            if (!success)
                return Conflict("Sædet er allerede reserveret.");

            return Ok("Sædevalg gemt midlertidigt.");
        }

        [HttpGet("selection")]
        public ActionResult<IEnumerable<Seat>> GetSelectedSeats([FromQuery] int bookingId)
        {
            var selectedSeats = _seatService.GetSelectedSeats(bookingId);

            if (!selectedSeats.Any())
                return NotFound("Ingen sæder valgt for denne booking.");

            return Ok(selectedSeats);
        }
    }
}
