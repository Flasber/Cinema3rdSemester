using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;

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
        public ActionResult<List<Screening>> GetAllScreenings()
        {
            return Ok(_screeningService.GetAllScreenings());
        }

        [HttpPost]
        public IActionResult AddScreening([FromBody] Screening screening)
        {
            _screeningService.AddScreening(screening);
            return CreatedAtAction(nameof(GetAllScreenings), new { id = screening.Id }, screening);

        }
    }
}
