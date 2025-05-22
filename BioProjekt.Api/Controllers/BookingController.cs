using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Shared.WebDtos;
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

        [HttpPost("createWithCustomer")]
        public async Task<ActionResult<Booking>> CreateBookingWithCustomer([FromBody] BookingCustomerCreateDTO dto)
        {
            var createdBooking = await _bookingService.CreateBookingWithCustomerAsync(dto);
            return Created("", createdBooking);
        }

    }
}