using CinemaBL;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginTokenController : ControllerBase
    {
        private readonly IConfiguration _conf;
        private readonly ITokenMng _token;
        private readonly CinemaContext _ctx;
        private readonly IUsersMng _userMng;

        public LoginTokenController(IConfiguration conf,
            ITokenMng token,
            CinemaDAL.Models.CinemaContext ctx,
            IUsersMng userMng)
        {
            _conf = conf;
            _token = token;
            _ctx = ctx;
            _userMng = userMng;
        }

        [AllowAnonymous]
        [HttpPost("CreateToken")]
        public IActionResult CreateToken([FromBody] LoginModel loginModel)
        {
            UserModel? um = _userMng.Autheticate(loginModel);
            if (um is not null)
            {
                var tokenString = _token.BuildToken(um, _conf);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

    }
}
