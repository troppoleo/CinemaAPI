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
        [Route("GetAll")]
        public IEnumerable<MovieDTO> GetAll()
        {
            return _ms.GetAll();
        }




        // [Route("~/CreateMovie2")]    --> "~" permette di cancellare tutto l'url 

        [HttpPost]
        [Route("CreateMovie")]
        public IActionResult  CreateMovie([FromBody] MovieForAddDTO movie)
        {            
            return Ok(_ms.Insert(movie).ToString());
        }

        [HttpPatch]
        [Route("UpdateMovie")]
        public ActionResult<string> UpdateMovie(MovieDTO movie)
        {            
            return Ok(_ms.Update(movie).ToString());
        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult<string> Delete(int idMovie)
        {            
            return Ok(_ms.Delete(idMovie).ToString());
        }

    }
}
