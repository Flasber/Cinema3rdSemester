using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<ScreeningSeat>> GetAvailableSeatsForScreeningAsync(int screeningId);
        Task<ScreeningSeat?> GetScreeningSeatByIdAsync(int screeningSeatId);
        Task<bool> TryReserveScreeningSeatAsync(int screeningSeatId, byte[] clientVersion);
        Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<ScreeningSeat> selectedSeats);
        Task CreateScreeningSeatsAsync(int screeningId, int auditoriumId);
        Task<IEnumerable<ScreeningSeat>> GetSelectedSeatsAsync(Guid sessionId);
        void StoreSeatSelection(Guid sessionId, ScreeningSeat screeningSeat);
        void ClearSeatSelection(Guid sessionId);
    }
}
