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
        // POST: /api/booking/createWithCustomer
        // Opretter en booking og tilhørende kunde baseret på input-DTO
        [HttpPost("createWithCustomer")]
        public async Task<ActionResult<Booking>> CreateBookingWithCustomer([FromBody] BookingCustomerCreateDTO dto)
        {
            try
            {
                var createdBooking = await _bookingService.CreateBookingWithCustomerAsync(dto);
                return Created("", createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Der opstod en serverfejl: " + ex.Message });
            }
        }


    }
}