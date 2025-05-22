using BioProjekt.Shared.WebDtos;
using BioProjektModels;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingWithCustomerAsync(BookingCustomerCreateDTO dto);
        Task<int> CreateBookingWithSeatsAsync(Guid sessionId, Booking booking);
    }
}
