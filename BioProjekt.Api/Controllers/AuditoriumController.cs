using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;
using System.Collections.Generic;

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
        public ActionResult<List<Auditorium>> GetAllAuditoriums()
        {
            return Ok(_auditoriumService.GetAllAuditoriums());
        }
    }
}
