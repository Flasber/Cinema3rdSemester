using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId);
        Task<Seat?> GetSeat(int seatNumber, string row, int auditoriumId);
        Task AddSeat(Seat seat);
        Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId);
        Task UpdateSeat(Seat seat);
        Task<IEnumerable<Seat>> GetSeatsForBookingAsync(int bookingId); 
        Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<Seat> selectedSeats);
    }

}
