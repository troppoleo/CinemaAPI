using CinemaBL;
using CinemaBL.Mapper;
using CinemaDTO;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTypesController : ControllerBase   //usare ControllerBase  per web API
    {
        private readonly IUserTypeService _uts;

        public UserTypesController(CinemaBL.IUserTypeService uts)
        {
            _uts = uts;
        }

        [HttpGet(Name = "GetUserTypes")]
        public IEnumerable<UserTypeDTO> GetUserTypes()
        {
            return _uts.GetUserType();
        }
    }
}
