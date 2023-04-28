using CinemaBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CinemaAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MngJobQualificationController : ControllerBase
    {
        private readonly IJobQualificationService _jqs;

        public MngJobQualificationController(CinemaBL.IJobQualificationService jqs)
        {
            _jqs = jqs;
        }


        [AllowAnonymous]
        [HttpGet(Name = "JobEmployeeQualification")]
        public ActionResult<IEnumerable<CinemaDTO.JobEmployeeQualificationMapDTO>> JobEmployeeQualification()
        {
            return Ok(_jqs.GetJobQualifications());
        }


        /// <summary>
        /// crea un nuono lavoro per i soli dipendenti
        /// create new employ
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>        
        [HttpPost("CreateNewJob"), Authorize(Roles = "ADMIN")]
        public IActionResult CreateNewJob([FromBody] CinemaDTO.JobEmployeeQualificationMinimalDTO job)
        {
            if (job.Description == null || job.ShortDescr == null)
            {
                return BadRequest();
            }
            _jqs.CreateNewJob(job);
            return Ok();
        }


        [HttpPatch, Authorize(Roles = "ADMIN")]
        public ActionResult UpdateJobEmployeeQualification([FromBody] CinemaDTO.JobEmployeeQualificationMapDTO job)
        {            
            if (_jqs.UpdateJobEmployeeQualification(job))
            {
                return Ok();
            }
            return NotFound();            
        }

        [HttpDelete, Authorize(Roles = "ADMIN")]
        public ActionResult DeleteJobEmployeeQualification([FromQuery] int idJobEmp)
        {
            return _jqs.DeleteJobEmployeeQualification(idJobEmp) ? Ok() : NotFound();
        }

    }
}
