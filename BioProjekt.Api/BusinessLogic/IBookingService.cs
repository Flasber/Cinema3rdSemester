using BioProjektModels;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking);
    }
}
