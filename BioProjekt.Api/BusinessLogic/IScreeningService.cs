using BioProjektModels;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IScreeningService
    {
        Task<List<Screening>> GetAllScreeningsAsync();
        Task AddScreeningAsync(Screening screening);
    }

}
