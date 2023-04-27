using CinemaBL;
using CinemaBL.Mapper;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserTypesController : ControllerBase   //usare ControllerBase  per web API
    {
        private readonly IUserTypeService _uts;

        public UserTypesController(CinemaBL.IUserTypeService uts)
        {
            _uts = uts;
        }

        //[AllowAnonymous] 
        [Authorize(Roles = "EMPLOYEE", Policy = "GET_TICKET")]
        [HttpGet(Name = "GetUserTypes")]
        public IEnumerable<UserTypeDTO> GetUserTypes()
        {
            return _uts.GetUserType();
        }
    }
}
