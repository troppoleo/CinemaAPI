using CinemaBL;
using CinemaBL.Repository;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "ADMIN")]
    [ApiController]
    public class MovieScheduleController : ControllerBase
    {
        private readonly IMovieScheduleService _mss;

        public MovieScheduleController(IMovieScheduleService mss)
        {
            _mss = mss;
        }



        [HttpPost]
        [Route("Insert"), AllowAnonymous]
        public IActionResult Insert(MovieScheduleForInsertDTO ms)
        {
            return Ok(_mss.Insert(ms).ToString());

        }

        [HttpPatch]
        [Route("Update"), AllowAnonymous]
        public IActionResult Update(MovieScheduleForUpdateDTO ms)
        { 
            return Ok(_mss.Update(ms).ToString());  

        }

        /// <summary>
        /// visualizza i film della giornata a partire dall'orario indicato
        /// Può visualizzare lo stato delle sale in diretta: il film in una certa sala è in corso oppure non è ancora iniziato/è finito quello in corso 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDirect"), AllowAnonymous]
        public IActionResult GetDirect(DateTime dateTime)
        {
            return Ok(_mss.GetDirect(dateTime));
        }

    }
}
