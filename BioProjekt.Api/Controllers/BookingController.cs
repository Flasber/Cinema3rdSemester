using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Dto.BookingDTO;

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
        public ActionResult<Booking> CreateBooking([FromBody] BookingCreateDTO dto)
        {
            var booking = new Booking
            {
                CustomerId = dto.CustomerId,
                ScreeningId = dto.ScreeningId,
                BookingDate = DateTime.Now,
                BookingStatus = "Pending",
               
            };

            var created = _bookingService.CreateBooking(booking);
            return Created("", created);
        }
    }
}
