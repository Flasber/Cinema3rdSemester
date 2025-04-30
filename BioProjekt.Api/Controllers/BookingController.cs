using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;

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

        [HttpGet]
        public ActionResult<List<Booking>> GetAllBookings()
        {
            return _bookingService.GetAllBookings();
        }

        [HttpPost]
        public ActionResult<Booking> CreateBooking([FromBody] Booking booking)
        {
            var created = _bookingService.CreateBooking(booking);
            return Created("", created);
        }
    }
}
