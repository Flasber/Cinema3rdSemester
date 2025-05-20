using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;

        public ScreeningController(IScreeningService screeningService)
        {
            _screeningService = screeningService;
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
            return CreatedAtAction(nameof(GetScreeningById), new { id = screening.Id }, createdScreening);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreeningById(int id)
        {
            var screening = await _screeningService.GetScreeningByIdAsync(id);

            if (screening == null)
                return NotFound($"Screening med id {id} blev ikke fundet.");

            return Ok(screening);
        }

    }
}
