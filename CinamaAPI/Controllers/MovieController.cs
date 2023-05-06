using CinemaBL;
using CinemaDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _ms;

        public MovieController(IMovieService ms)
        {
            _ms = ms;
        }

        [HttpGet]
        [Route("GetAllMovie")]
        public async Task<IEnumerable<MovieDTO>> GetAllMovie()
        {
            return await _ms.GetAllMovie();
        }



        [HttpPost]
        [Route("CreateMovieAsync")]
        public async Task<ActionResult<string>> CreateMovieAsync([FromBody] MovieForAddDTO movie)
        {
            var se = await _ms.CreateMovieAsync(movie);

            return Ok(se.ToString());
        }


        // [Route("~/CreateMovie2")]    --> "~" permette di cancellare tutto l'url 

        [HttpPost]
        [Route("CreateMovie")]
        public ActionResult<string> CreateMovie([FromBody] MovieForAddDTO movie)
        {
            var se = _ms.CreateMovie(movie);

            return Ok(se.ToString());
        }

        [HttpPatch]
        [Route("UpdateMovie")]
        public ActionResult<string> UpdateMovie(MovieDTO movie)
        {
            var se = _ms.UpdateMovie(movie);
            return Ok(se.ToString());
        }

        [HttpDelete]
        [Route("DeleteMovie")]
        public ActionResult<string> DeleteMovie(int idMovie)
        {
            var se = _ms.DeleteMovie(idMovie);
            return Ok(se.ToString());
        }

    }
}
