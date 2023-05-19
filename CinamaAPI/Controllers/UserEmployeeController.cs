using CinemaBL;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CinemaAPI.Controllers
{
    
    [Route("api/[controller]"), Authorize(Roles = "ADMIN")]
    [ApiController]
    public class UserEmployeeController : ControllerBase
    {
        private readonly IUserEmployeeService _uu;

        public UserEmployeeController(IUserEmployeeService uu)
        {
            _uu = uu;
        }

        [HttpPost]
        [Route("Insert")]
        public string Insert(UserEmployeeForInsertDTO ued)
        {
            return _uu.Insert(ued).ToString();
        }

        //[HttpPost]
        //[Route("AddMinimal")]
        //public void AddMinimal(UsersEmployeeMinimalDTO uedMin)
        //{
        //    _uu.AddMinimal(uedMin);
        //}

        [HttpPatch]
        [Route("Update")]
        public IActionResult Update(UserEmployeeDTO ued)
        {
            return Ok(_uu.Update(ued).ToString());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_uu.Delete(id).ToString());
        }

        [HttpGet]
        [Route("Get/{id}")]
        public UserEmployeeDTO Get(int id)
        {
            return _uu.Get(id);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<UserEmployeeDTO> GetAll()
        {
            return _uu.GetAll();
        }
    }
}
