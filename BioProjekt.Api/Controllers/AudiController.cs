using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjektModels.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudiController : ControllerBase
    {
        private readonly ICinemaRepository _cinemaRepository;

        public AudiController(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        // GET: /api/audi
        // Returns all auditoriums in the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auditorium>>> GetAllAuditoriums()
        {
            var auditoriums = _cinemaRepository.GetAllAuditoriums();
            return Ok(auditoriums);
        }
    }
}