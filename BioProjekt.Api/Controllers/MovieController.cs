using Microsoft.AspNetCore.Mvc;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = new List<string> { "Inception", "Interstellar", "The Matrix" };
            return Ok(movies);
     
        
        }
       //noget fra timen gem det gem det ikke IDC
      //sætte en route op...
      // [HttpGet]
    }
}
