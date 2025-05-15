using BioProjektModels;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> CreateBookingAsync(Booking booking);
        Task AddSeatToBookingAsync(int bookingId, int seatId);
    }
}
