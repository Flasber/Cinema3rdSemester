using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [HttpGet]
        public IActionResult GetAllShowtimes()
        {
            var movies = _movieRepository.GetAllMoviesAsync().Result.ToList();
            var screenings = _screeningRepository.GetAllScreeningsAsync().Result.ToList();
            var auditoriums = _auditoriumRepository.GetAllAuditoriumsAsync().Result.ToList();

            var showtimes = screenings.Select(s => new
            {
                s.Id,
                s.Date,
                s.Time,
                MovieTitle = movies.FirstOrDefault(m => m.Id == s.MovieId)?.Title,
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
