using BioProjektModels;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IBookingService
    {
        Booking CreateBooking(Booking booking);
        List<Booking> GetAllBookings();
    }
}
