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

        [HttpGet(Name = "GetAllMovie")]
        public async Task<IEnumerable<MovieDTO>> GetAllMovie()
        {
            return await _ms.GetAllMovie();
        }

        //[HttpPost(Name = "CreateMovie")]
        //public ActionResult<string> CreateMovie([FromBody] MovieDTO movie)
        //{
        //    var se = _ms.CreateMovie(movie);

        //    return Ok(se.ToString());
        //}

        
        //[HttpPatch(Name = "UpdateMovie")]
        //public ActionResult<string> UpdateMovie(MovieDTO movie)
        //{
        //    var se = _ms.UpdateMovie(movie);
        //    return Ok(se.ToString());
        //}

    }
}
