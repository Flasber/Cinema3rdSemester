using Microsoft.AspNetCore.Mvc;
using BioProjekt.Api.BusinessLogic;
using BioProjektModels;

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
        public ActionResult<List<Movie>> GetMovies()
        {
            var movies = _movieService.GetAllMovies();
            return Ok(movies);
        }
    }
}
