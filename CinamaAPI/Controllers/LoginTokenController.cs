using CinemaAPI.Hubs;
using CinemaBL;
using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginTokenController : ControllerBase
    {
        private readonly IConfiguration _conf;
        private readonly ITokenMng _token;
        private readonly IUsersMng _userMng;
        private readonly IHubContext<CinemaHub> _hub;

        public LoginTokenController(IConfiguration conf,
            ITokenMng token,
            IUsersMng userMng,
            IHubContext<CinemaHub> hub)
        {
            _conf = conf;
            _token = token;
            _userMng = userMng;
            _hub = hub;
        }

        [AllowAnonymous]
        [HttpPost(Name = "PostCreateToken")]
        public IActionResult PostCreateToken([FromBody] LoginModelDTO loginModel)
        {
            UserModelDTO? um = _userMng.Autheticate(loginModel);
            if (um is not null)
            {
                var tokenString = _token.BuildToken(um, _conf);

                //if (um.JobQualification.ToEnum<JobEmployeeQualificationEnum>() == JobEmployeeQualificationEnum.OWN_SALA)
                //{
                //    //_hub.Clients.
                //    //xx.AddToGroup(um);
                //}

                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

    }
}
