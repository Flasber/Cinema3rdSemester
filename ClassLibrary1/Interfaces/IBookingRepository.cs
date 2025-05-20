using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> CreateBookingAsync(Booking booking);
        Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<ScreeningSeat> selectedSeats);
    }
}
