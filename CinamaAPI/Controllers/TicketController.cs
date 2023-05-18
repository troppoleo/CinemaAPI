using CinemaBL;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ts;

        public TicketController(ITicketService ts )
        {
            _ts = ts;
        }


        [HttpPost]
        [Route("TicketGenerate")]
        [Authorize(Roles ="EMPLOYEE", Policy ="GET_TICKET")]
        public ActionResult<TicketGenerateResultDTO> TicketGenerate([FromBody] TicketGenerateDTO tg)
        {
            return Ok(_ts.TicketGenerate(tg));
        }
    }
}
