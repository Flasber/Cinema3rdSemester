using Microsoft.AspNetCore.Mvc;
using BioProjekt.Api.BusinessLogic;
using BioProjektModels;
using System.Collections.Generic;

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

        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovieById(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        [HttpGet("{id}/genre")]
        public ActionResult<string> GetMovieGenre(int id)
        {
            var genre = _movieService.GetMovieGenre(id);
            if (genre == null)
                return NotFound();
            return Ok(genre);
        }

        [HttpGet("{id}/description")]
        public ActionResult<string> GetMovieDescription(int id)
        {
            var description = _movieService.GetMovieDescription(id);
            if (description == null)
                return NotFound();
            return Ok(description);
        }

        [HttpGet("{id}/duration")]
        public ActionResult<TimeSpan> GetMovieDuration(int id)
        {
            var duration = _movieService.GetMovieDuration(id);
            if (duration == TimeSpan.Zero)
                return NotFound();
            return Ok(duration);
        }
    }
}
