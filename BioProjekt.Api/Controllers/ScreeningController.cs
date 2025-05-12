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
            return CreatedAtAction(nameof(GetAllScreenings), new { id = screening.Id }, screening);
        }
    }
}
