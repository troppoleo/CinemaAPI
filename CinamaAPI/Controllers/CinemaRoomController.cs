using CinemaBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaRoomController : ControllerBase
    {
        private readonly ICinemaRoomService _cr;

        public CinemaRoomController(CinemaBL.ICinemaRoomService cr)
        {
            _cr = cr;
        }

        [HttpGet(Name = "GetCinemaRooms")]
        public IEnumerable<CinemaDAL.Models.CinemaRoom> GetCinemaRooms()
        {
            return _cr.GetCinemaRooms();
        }

        /// <summary>
        /// o	Per creare una sala serve un Responsabile di Sala da associare (uno solo per sala) 
        /// 	Non può esserci lo stesso per più sale
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateCinemaRoom")]
        public ActionResult<string> CreateCinemaRoom(CinemaDTO.CreateCinemaDTO cc)
        {
            return "";
        }

        /// <summary>
        /// Una sala NON può essere eliminata se c’è un film programmato in quella sala 
        /// Se viene eliminata, il Responsabile di Sala NON viene eliminato ma rimane “libero”  
        /// </summary>
        [HttpDelete(Name = "DeleteCinemaRoom")]
        public ActionResult<string> DeleteCinemaRoom([FromQuery] int id)
        {
            return "";
        }


        ///o	I posti, posti vip ecc, possono essere modificati solo quando non ci sono in programma film approvati 
        ///         in quella sala o Il numero e il nome (facoltativo) devono essere unici
        ///	Il numero può essere cambiato ma non può essere< 1 
        [HttpPatch(Name = "UpdateCinemaRoom")]
        public ActionResult<string> UpdateCinemaRoom([FromBody] CinemaDTO.CreateCinemaDTO cc)
        {
            return "";
        }


    }
}
