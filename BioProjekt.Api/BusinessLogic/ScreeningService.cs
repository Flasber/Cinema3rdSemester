using BioProjektModels;
using BioProjektModels.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BioProjekt.Api.BusinessLogic
{
    public class ScreeningService : IScreeningService
    {
        private readonly ISqlCinemaRepository _cinemaRepository;
        private readonly List<Screening> _extraScreenings = new();
        private int _nextId;

        public ScreeningService(ISqlCinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
            var existingScreenings = _cinemaRepository.GetAllScreenings().Result;
            _nextId = existingScreenings.Any() ? existingScreenings.Max(s => s.Id) + 1 : 1;
        }

        public List<Screening> GetAllScreenings()
        {
            var existingScreenings = _cinemaRepository.GetAllScreenings().Result.ToList();
            return existingScreenings.Concat(_extraScreenings).ToList();
        }

        public void AddScreening(Screening screening)
        {
            screening.Id = _nextId++;
            _extraScreenings.Add(screening);
        }
    }
}
