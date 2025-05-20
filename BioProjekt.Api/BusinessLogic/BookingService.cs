using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Api.Storage;
using BioProjekt.Shared.WebDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly SeatSelectionStore _seatSelectionStore;

        public BookingService(
            IBookingRepository bookingRepo,
            ICustomerRepository customerRepo,
            IScreeningRepository screeningRepo,
            SeatSelectionStore seatSelectionStore)
        {
            _bookingRepository = bookingRepo;
            _customerRepository = customerRepo;
            _screeningRepository = screeningRepo;
            _seatSelectionStore = seatSelectionStore;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            booking.BookingDate = DateTime.Now;
            booking.BookingId = await _bookingRepository.CreateBookingAsync(booking);
            return booking;
        }

        public async Task<Booking> CreateBookingWithCustomerAsync(BookingCustomerCreateDTO dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                MobileNumber = dto.MobileNumber,
                Address = dto.Address,
                CustomerType = dto.CustomerType
            };

            var customerNumber = await _customerRepository.CreateCustomerAsync(customer);

            var booking = new Booking
            {
                ScreeningId = dto.ScreeningId,
                BookingDate = DateTime.Now,
                BookingStatus = "Pending",
                Price = 65,
                IsDiscounted = false,
                CustomerNumber = customerNumber
            };

            booking.BookingId = await _bookingRepository.CreateBookingAsync(booking);

            var selectedSeats = _seatSelectionStore.GetSeats(dto.SessionId);

            if (!selectedSeats.Any())
                throw new InvalidOperationException("Ingen sæder valgt.");

            await _bookingRepository.AssignSeatsToBooking(dto.SessionId, booking.BookingId, selectedSeats.ToList());

            _seatSelectionStore.Clear(dto.SessionId);

            return booking;
        }

    }
}
