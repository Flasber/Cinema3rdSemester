using BioProjektModels;
using BioProjekt.Api.Dto.SeatDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface ISeatService
    {
        Task AddSeat(Seat seat);
        Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId);
        Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId);
        Task<IEnumerable<SeatAvailability>> GetAvailableSeats(int auditoriumId);
        bool SelectSeat(Guid sessionId, int seatNumber, string row, int auditoriumId);
        IEnumerable<Seat> GetSelectedSeats(Guid sessionId);
        Task AssignSeatsToBooking(Guid sessionId, int bookingId);
    }
}
