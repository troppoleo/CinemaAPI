using CinemaBL;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "ADMIN")]
    [ApiController]
    public class JobEmployeeQualificationController : ControllerBase
    {
        private readonly IJobEmployeeQualificationService _jbe;

        public JobEmployeeQualificationController(IJobEmployeeQualificationService jbe)
        {
            _jbe = jbe;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            return Ok(_jbe.GetAll());
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_jbe.Delete(id).ToString());
        }


        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetByID(int id)
        {
            return Ok(_jbe.GetByID(id));
        }

        [HttpPatch]
        [Route("Update")]
        public IActionResult Update(JobEmployeeQualificationDTO jeq)
        { 
            return Ok(_jbe.Update( jeq).ToString());
        }

        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert(JobEmployeeQualificationForInsertDTO jeq)
        { 
            return Ok(_jbe.Insert(jeq).ToString());
        }
    }
}
