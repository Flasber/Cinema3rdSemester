using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface IScreeningRepository
    {
        Task<IEnumerable<Screening>> GetAllScreeningsAsync();
        Task AddScreeningAsync(Screening screening);

    }
}
