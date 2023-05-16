using CinemaAPI.Hubs;
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
        private readonly INotify _nf;

        public MovieScheduleController(IMovieScheduleService mss, INotify nf)
        {
            _mss = mss;
            _nf = nf;
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
            var result = _mss.Update(ms);

            if (ms.IsApproved == 1)
            {
                switch (result)
                {
                    case CinemaBL.Enums.CrudCinemaEnum.UPDATED:
                        _nf.SendMessageToGET_TICKET(ms.Id);
                        break;
                }
            }
            return Ok(result.ToString());  

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
