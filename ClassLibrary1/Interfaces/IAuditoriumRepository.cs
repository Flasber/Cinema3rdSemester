using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Interfaces
{
    public interface IAuditoriumRepository
    {
        Task<IEnumerable<Auditorium>> GetAllAuditoriumsAsync();
    }
}
