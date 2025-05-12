using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Dto.BookingDTO;
using System.Threading.Tasks;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingCreateDTO dto)
        {
            var booking = new Booking
            {
                CustomerNumber = dto.CustomerNumber,
                ScreeningId = dto.ScreeningId,
                BookingStatus = "Pending"
            };

            var created = await _bookingService.CreateBookingAsync(booking);
            return Created("", created);
        }
    }
}
