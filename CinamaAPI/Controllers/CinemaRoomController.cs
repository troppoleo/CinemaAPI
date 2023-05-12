using CinemaBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Formats.Asn1;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "ADMIN")]
    [ApiController]
    public class CinemaRoomController : ControllerBase
    {
        private readonly ICinemaRoomService _cr;

        public CinemaRoomController(CinemaBL.ICinemaRoomService cr)
        {
            _cr = cr;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<IEnumerable<CinemaDTO.CinemaRoomDTO>> GetAll()
        {
            return Ok(_cr.GetAll());
        }

        [HttpGet]
        [Route("GetByID/{id}")]
        public ActionResult<CinemaDTO.CinemaRoomDTO>? GetByID(int id)
        {
            return Ok(_cr.GetByID(id));
        }

        /// <summary>
        /// o	Per creare una sala serve un Responsabile di Sala da associare (uno solo per sala) 
        /// 	Non può esserci lo stesso per più sale
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IActionResult Insert(CinemaDTO.CinemaRoomForInsertDTO cc)
        //{
        //    return Ok(_cr.Insert(cc).ToString());
        //}


        /// <summary>
        /// crea una sala con il responsabile di sala
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertWithOwn"), AllowAnonymous]
        public IActionResult InsertWithOwn(CinemaDTO.CinemaRoomForInsertWithOwnDTO cc)
        {
            return Ok(_cr.InsertWithOwn(cc).ToString());
        }


        /// <summary>
        /// Una sala NON può essere eliminata se c’è un film programmato in quella sala 
        /// Se viene eliminata, il Responsabile di Sala NON viene eliminato ma rimane “libero”  
        /// </summary>
        [HttpDelete]
        [Route("Delete/{id}"), AllowAnonymous]
        public ActionResult<string> Delete(int id)
        {
            return _cr.Delete(id).ToString();
        }


        ///o	I posti, posti vip ecc, possono essere modificati solo quando non ci sono in programma film approvati 
        ///         in quella sala o Il numero e il nome (facoltativo) devono essere unici
        ///	Il numero può essere cambiato ma non può essere< 1 
        [HttpPatch]
        [Route("Update"), AllowAnonymous]
        public ActionResult<string> Update([FromBody] CinemaDTO.CinemaRoomDTO cc)
        {
            return _cr.Update(cc).ToString();
        }


    }
}
