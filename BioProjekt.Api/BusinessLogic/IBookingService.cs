using BioProjekt.Shared.WebDtos;
using BioProjektModels;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> CreateBookingWithCustomerAsync(BookingCustomerCreateDTO dto);
    }
}