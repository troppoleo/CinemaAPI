using CinemaBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static CinemaBL.UsersMng;

namespace CinemaAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUsersMng _um;

        public EmployeeController(CinemaBL.IUsersMng um)
        {
            _um = um;
        }

        [HttpGet(Name = "GetEmplyee"), AllowAnonymous]
        public IEnumerable<CinemaDTO.UsersEmployeeDTO> GetEmplyee()
        {
            return _um.GetUsersEmployee();
        }



        /// <summary>
        /// ricerca Employee via Username 
        /// </summary>
        /// <param name="userName">insensitive case</param>
        /// <returns></returns>
        [HttpGet(Name = "GetEmployeeByUserName"), AllowAnonymous]
        public IEnumerable<CinemaDTO.UsersEmployeeDTO> GetUsersEmployeeByUserName(string userName)
        {
            return _um.GetUsersEmployeeByUserName(userName);
        }


        [HttpPost(Name = "CreateNewEmployee")]
        public ActionResult<string> CreateNewEmployee([FromBody] CinemaDTO.UsersEmployeeMinimalDTO emp)
        {
            UsersMngEnum res = UsersMngEnum.NONE;
            try
            {
                res = _um.CreateEmployee(emp);
                return Ok(res.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch(Name = "UpdateEmployee")]
        public ActionResult<string> UpdateEmployee([FromBody] CinemaDTO.UsersEmployeeDTO emp)
        {
            try
            {
                var r = _um.UpdateEmployee(emp);
                return Ok(r.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPatch(Name = "UpdateEmployeeJob")]
        public ActionResult<string> UpdateEmployeeJob([FromBody] CinemaDTO.UsersEmployeeJobDTO emp)
        {
            try
            {                
                return Ok(_um.UpdateEmployeeJob(emp).ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(Name = "DeleteEmployeeJob")]
        public ActionResult<string> DeleteEmployeeJob([FromQuery] int id)
        {
            return Ok(_um.DeleteEmployeeJob(id));
        }




    }
}
