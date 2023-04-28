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



        [Authorize(Roles = "EMPLOYEE")]
        [HttpGet(Name = "GetUserTypes_EMPLOYEE")]
        public IEnumerable<UserTypeDTO> GetUserTypes_EMPLOYEE()
        {
            return _uts.GetUserType();
        }

        //[AllowAnonymous] 
        [Authorize(Roles = "EMPLOYEE", Policy = "GET_TICKET")]
        [HttpGet(Name = "GetUserTypes_EMPLOYEE_GET_TICKET")]
        public IEnumerable<UserTypeDTO> GetUserTypes_EMPLOYEE_GET_TICKET()
        {
            return _uts.GetUserType();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet(Name = "GetUserTypes_ADMIN")]
        public IEnumerable<UserTypeDTO> GetUserTypes_ADMIN()
        {
            foreach (var x in User.Claims)
            {
                string type = x.Type;
                string val = x.Value;
            }
            return _uts.GetUserType();
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetUserTypes_AllowAnonymous")]
        public IEnumerable<UserTypeDTO> GetUserTypes_AllowAnonymous()
        {
            return _uts.GetUserType();
        }




    }
}
