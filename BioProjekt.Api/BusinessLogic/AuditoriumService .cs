using BioProjektModels.Interfaces;
using BioProjektModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly ISqlCinemaRepository _cinemaRepository;

        public AuditoriumService(ISqlCinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<List<Auditorium>> GetAllAuditoriums()
        {
            var auditoriums = await _cinemaRepository.GetAllAuditoriums();
            return auditoriums.ToList();
        }
    }
}
