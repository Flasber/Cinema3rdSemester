using BioProjektModels;
using BioProjekt.Shared.WebDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatAvailability>> GetAvailableSeats(int screeningId);
        Task<bool> TryReserveScreeningSeat(int screeningSeatId, byte[] clientVersion);
        Task<bool> SelectSeats(Guid sessionId, List<int> screeningSeatIds);
        IEnumerable<ScreeningSeat> GetSelectedSeats(Guid sessionId);
        Task AssignSeatsToBooking(Guid sessionId, int bookingId);
    }
}
