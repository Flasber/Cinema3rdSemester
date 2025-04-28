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

        [HttpGet("genre/{genreName}")]
        public ActionResult<List<Movie>> GetMoviesByGenre(string genreName)
        {
            var movies = _movieService.GetMoviesByGenre(genreName);
            if (movies == null || movies.Count == 0)
                return NotFound();
            return Ok(movies);
        }



    }
}
