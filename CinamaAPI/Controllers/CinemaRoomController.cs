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

    }
}
