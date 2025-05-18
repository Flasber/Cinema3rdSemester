using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using System.Threading.Tasks;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Title = dto.Title,
                Genre = dto.Genre,
                Duration = dto.Duration,
                Description = dto.Description,
                Language = dto.Language,
                AgeRating = dto.AgeRating,
                PosterUrl = dto.PosterUrl
            };

            await _movieService.CreateMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
    }
}
