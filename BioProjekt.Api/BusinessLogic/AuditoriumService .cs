using BioProjektModels;
using BioProjekt.Api.Data.Mockdatabase;
using System.Collections.Generic;
using System.Linq;

namespace BioProjekt.Api.BusinessLogic
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly ICinemaRepository _cinemaRepository;

        public AuditoriumService(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public List<Auditorium> GetAllAuditoriums()
        {
            return _cinemaRepository.GetAllAuditoriums().ToList();
        }
    }
}
