using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Api.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningRepository _screeningRepository;

        public ScreeningService(IScreeningRepository screeningRepository)
        {
            _screeningRepository = screeningRepository;
        }

        public async Task<List<Screening>> GetAllScreeningsAsync()
        {
            var screenings = await _screeningRepository.GetAllScreeningsAsync();
            return screenings.ToList();
        }

        public async Task AddScreeningAsync(Screening screening)
        {
            await _screeningRepository.AddScreeningAsync(screening);
        }
    }

}
