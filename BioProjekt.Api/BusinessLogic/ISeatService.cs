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
        IEnumerable<ScreeningSeat> GetSelectedSeats(Guid sessionId);
    }
}
