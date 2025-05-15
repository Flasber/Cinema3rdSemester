using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BioProjekt.DataAccess.Interfaces;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/showtime")]
    public class ShowtimeController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IScreeningRepository _screeningRepository;
        private readonly IAuditoriumRepository _auditoriumRepository;

        public ShowtimeController(
            IMovieRepository movieRepository,
            IScreeningRepository screeningRepository,
            IAuditoriumRepository auditoriumRepository)
        {
            _movieRepository = movieRepository;
            _screeningRepository = screeningRepository;
            _auditoriumRepository = auditoriumRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowtimesByMovieId(int id)
        {
            var movie = (await _movieRepository.GetAllMoviesAsync()).FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound($"Movie with id {id} not found.");
            }

            var screenings = (await _screeningRepository.GetAllScreeningsAsync())
                .Where(s => s.MovieId == id)
                .ToList();

            var auditoriums = await _auditoriumRepository.GetAllAuditoriumsAsync();

            var showtimes = screenings.Select(s => new
            {
                s.Id,
                s.Date,
                s.Time,
                MovieTitle = movie.Title,
                AuditoriumName = auditoriums.FirstOrDefault(a => a.Id == s.AuditoriumId)?.Name,
                s.LanguageVersion,
                s.Is3D,
                s.IsSoldOut,
                s.SoundSystem
            });

            return Ok(showtimes);
        }

    }
}
