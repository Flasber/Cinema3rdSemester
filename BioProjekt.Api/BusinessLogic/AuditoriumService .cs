using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Api.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioProjektModels.Interfaces;

namespace BioProjekt.Api.BusinessLogic
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumRepository _auditoriumRepository;

        public AuditoriumService(IAuditoriumRepository auditoriumRepository)
        {
            _auditoriumRepository = auditoriumRepository;
        }

        public async Task<List<Auditorium>> GetAllAuditoriums()
        {
            var auditoriums = await _auditoriumRepository.GetAllAuditoriumsAsync();
            return auditoriums.ToList();
        }
    }
}
