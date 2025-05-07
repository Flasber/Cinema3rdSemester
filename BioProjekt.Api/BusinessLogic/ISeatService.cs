using System.Collections.Generic;
using BioProjekt.Api.Dto.SeatDTO;
using BioProjektModels;

namespace BioProjekt.Api.BusinessLogic
{
    public interface ISeatService
    {
        void AddSeat(Seat seat);
        bool TryReserveSeat(int seatNumber, string row, int clientVersion, int auditoriumId);
        IEnumerable<Seat> GetSeatsForAuditorium(int auditoriumId);
        IEnumerable<SeatAvailability> GetAvailableSeats(int auditoriumId);
        bool SelectSeatForBooking(int bookingId, int seatNumber, string row, int auditoriumId);
        IEnumerable<Seat> GetSelectedSeats(int bookingId);
    }
}
