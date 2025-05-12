using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Api.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            booking.BookingDate = DateTime.Now;
            booking.Id = await _bookingRepository.CreateBookingAsync(booking);
            return booking;
        }
    }
}
