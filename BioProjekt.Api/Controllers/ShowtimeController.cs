using BioProjekt.Api.Data.Mockdatabase;
using Microsoft.AspNetCore.Mvc;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/showtime")]
    public class ShowtimeController : ControllerBase
    {
        private readonly ICinemaRepository _repo;

        public ShowtimeController(ICinemaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAllShowtimes()
        {
            var movies = _repo.GetAllMovies().ToList();
            var auditoriums = _repo.GetAllAuditoriums().ToList();

            var showtimes = _repo.GetAllScreenings()
                .Select(s => new
                {
                    s.Id,
                    s.Time,
                    s.Date,
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
