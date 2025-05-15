using BioProjektModels;
using BioProjekt.Shared.WebDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId);
        Task<IEnumerable<SeatAvailability>> GetAvailableSeats(int auditoriumId);
        Task AddSeat(Seat seat);
        Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId);
        bool SelectSeat(Guid sessionId, int seatNumber, string row, int auditoriumId);
        IEnumerable<Seat> GetSelectedSeats(Guid sessionId);
        Task AssignSeatsToBooking(Guid sessionId, int bookingId);
    }
}
