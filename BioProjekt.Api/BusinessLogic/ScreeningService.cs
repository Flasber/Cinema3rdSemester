using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningRepository _screeningRepository;
        private readonly ISeatRepository _seatRepository;

        public ScreeningService(IScreeningRepository screeningRepository, ISeatRepository seatRepository)
        {
            _screeningRepository = screeningRepository;
            _seatRepository = seatRepository;
        }

        public async Task<List<Screening>> GetAllScreeningsAsync()
        {
            var screenings = await _screeningRepository.GetAllScreeningsAsync();
            return screenings.ToList();
        }

        public async Task AddScreeningAsync(Screening screening)
        {
            await _screeningRepository.AddScreeningAsync(screening);
            await _seatRepository.CreateScreeningSeatsAsync(screening.Id, screening.AuditoriumId);
        }

        public async Task<Screening?> GetScreeningByIdAsync(int id)
        {
            var screenings = await _screeningRepository.GetAllScreeningsAsync();
            return screenings.FirstOrDefault(s => s.Id == id);
        }
    }
}
