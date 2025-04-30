using BioProjektModels;
using System;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public class BookingService : IBookingService
    {
        private static readonly List<Booking> Bookings = new();
        private static int _id = 1;

        public Booking CreateBooking(Booking booking)
        {
            booking.Id = _id++;
            booking.BookingDate = DateTime.Now;
            Bookings.Add(booking);
            return booking;
        }

        public List<Booking> GetAllBookings()
        {
            return Bookings;
        }
    }
}
