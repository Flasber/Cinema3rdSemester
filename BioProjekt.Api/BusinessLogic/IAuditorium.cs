using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BioProjektModels.Interfaces
{
    public interface IAuditoriumService
    {
        Task<List<Auditorium>> GetAllAuditoriums();
    }
}
