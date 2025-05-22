using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioProjekt.Shared.WebDtos;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly IMovieService _movieService;

        public ScreeningController(IScreeningService screeningService, IMovieService movieService)
        {
            _screeningService = screeningService;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Screening>>> GetAllScreenings()
        {
            var screenings = await _screeningService.GetAllScreeningsAsync();
            return Ok(screenings);
        }

        [HttpPost]
        public async Task<IActionResult> AddScreening([FromBody] Screening screening)
        {
            await _screeningService.AddScreeningAsync(screening);
            var createdScreening = await _screeningService.GetScreeningByIdAsync(screening.Id);
            return CreatedAtAction(nameof(GetScreening), new { id = screening.Id }, createdScreening);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScreeningWebDto>> GetScreening(int id)
        {
            var screening = await _screeningService.GetScreeningByIdAsync(id);
            if (screening == null) return NotFound();

            var movie = await _movieService.GetMovieByIdAsync(screening.MovieId);
            if (movie == null) return NotFound("Filmen blev ikke fundet");

            var dto = new ScreeningWebDto
            {
                Id = screening.Id,
                StartDateTime = screening.StartDateTime,
                EndDateTime = screening.StartDateTime.AddMinutes(movie.Duration),
                MovieId = screening.MovieId,
                AuditoriumId = screening.AuditoriumId
            };

            return Ok(dto);
        }
    }
}
