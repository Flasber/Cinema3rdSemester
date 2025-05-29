using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using System.Threading.Tasks;
using BioProjekt.Shared.ClientDtos;
using System.Collections.Generic;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IScreeningService _screeningService;

        public MovieController(IMovieService movieService, IScreeningService screeningService)
        {
            _movieService = movieService;
            _screeningService = screeningService;
        }
        // GET: /api/movie
        // Returnerer alle film i databasen
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }
        // GET: /api/movie/{id}
        // Returnerer detaljer for en enkelt film ud fra dens ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        // POST: /api/movie
        // Opretter en ny film og tilknyttede forestillinger
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

            foreach (var screeningDto in dto.Screenings)
            {
                var screening = new Screening
                {
                    MovieId = movie.Id,
                    Date = screeningDto.Date,
                    Time = screeningDto.Time,
                    LanguageVersion = screeningDto.LanguageVersion,
                    Is3D = screeningDto.Is3D,
                    IsSoldOut = false,
                    SoundSystem = screeningDto.SoundSystem,
                    AuditoriumId = screeningDto.AuditoriumId
                };

                await _screeningService.AddScreeningAsync(screening);
            }

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
        // POST: /api/movie/upload-poster
        // Modtager og gemmer en filmplakat som billede i wwwroot/images og returnerer sti
        [HttpPost("upload-poster")]
        public async Task<ActionResult<string>> UploadPoster(IFormFile poster, [FromServices] IWebHostEnvironment env)
        {
            if (poster == null || poster.Length == 0)
                return BadRequest("Ingen fil modtaget.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(poster.FileName)}";
            var savePath = Path.Combine(env.WebRootPath, "images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await poster.CopyToAsync(stream);
            }

            var relativeUrl = $"/images/{fileName}";
            return Ok(relativeUrl);
        }

    }
}
