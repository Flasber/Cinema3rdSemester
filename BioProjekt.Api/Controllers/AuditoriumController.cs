using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjektModels.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriumController : ControllerBase
    {
        private readonly IAuditoriumService _auditoriumService;

        public AuditoriumController(IAuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Auditorium>>> GetAllAuditoriums()
        {
            var auditoriums = await _auditoriumService.GetAllAuditoriums();
            return Ok(auditoriums);
        }
    }
}
