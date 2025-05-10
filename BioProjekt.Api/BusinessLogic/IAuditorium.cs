using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjektModels.Interfaces
{
    public interface IAuditoriumService
    {
        Task<List<Auditorium>> GetAllAuditoriums();
    }
}
