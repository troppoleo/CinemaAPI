using CinemaBL;
using CinemaBL.Enums;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using static CinemaBL.UsersMng;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUsersMng _um;
        private readonly IUserEmployeeService _ues;

        public EmployeeController(CinemaBL.IUsersMng um, IUserEmployeeService ues)
        {
            _um = um;
            _ues = ues;
        }


        [HttpGet, AllowAnonymous]
        [Route("GetAll2")]
        public IEnumerable<CinemaDTO.UserEmployeeDTO> GetAll2()
        {

            return _ues.GetAll();
        }

        [HttpGet, AllowAnonymous]
        [Route("GetEmplyee")]
        public IEnumerable<CinemaDTO.UserEmployeeDTO> GetEmplyee()
        {

            return _um.GetUsersEmployee();
        }



        /// <summary>
        /// ricerca Employee via Username 
        /// </summary>
        /// <param name="userName">insensitive case</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        [Route("GetEmployeeByUserName")]
        public IEnumerable<CinemaDTO.UserEmployeeDTO> GetUsersEmployeeByUserName(string userName)
        {
            return _um.GetUsersEmployeeByUserName(userName);
        }


        [Obsolete]
        [HttpPost]
        [Route("CreateNewEmployee")]
        public ActionResult<string> CreateNewEmployee([FromBody] CinemaDTO.UserEmployeeMinimalDTO emp)
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

        //[HttpPost]
        //[Route("Add")]
        //public ActionResult<CinemaEnum> Add(UsersEmployeeDTO ue)
        //{
        //    return Ok(_ues.Add(ue));
        //}

        //[HttpPost]
        //[Route("AddMinimal")]
        //public ActionResult<CinemaEnum> AddMinimal(UsersEmployeeMinimalDTO ue)
        //{
        //    return Ok(_ues.AddMinimal(ue));
        //}

        //[HttpPost]
        //[Route("Update")]
        //public ActionResult<CinemaEnum> Update(UsersEmployeeDTO ue)
        //{
        //    return Ok(_ues.Update(ue));
        //}




        [HttpPatch]
        [Route("UpdateEmployee")]
        public ActionResult<string> UpdateEmployee([FromBody] CinemaDTO.UserEmployeeDTO emp)
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

        [HttpPatch]
        [Route("UpdateEmployeeJob")]
        public ActionResult<string> UpdateEmployeeJob([FromBody] CinemaDTO.UserEmployeeJobDTO emp)
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

        [HttpDelete]
        [Route("DeleteEmployeeJob")]
        public ActionResult<string> DeleteEmployeeJob([FromQuery] int id)
        {
            return Ok(_um.DeleteEmployeeJob(id));
        }

    }
}
