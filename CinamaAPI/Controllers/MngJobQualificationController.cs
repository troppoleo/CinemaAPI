using CinemaBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CinemaAPI.Controllers
{
    /// <summary>
    /// interagisce con la tabella: "JobEmployeeQualifications"
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class MngJobQualificationController : ControllerBase
    {
        private readonly IJobQualificationService _jqs;

        public MngJobQualificationController(CinemaBL.IJobQualificationService jqs)
        {
            _jqs = jqs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Restituisce la lista delle varie qualifiche (responsabile sala, bigliettaio) per gli EMPLOYEE </returns>
        [AllowAnonymous]
        [HttpGet(Name = "JobEmployeeQualification")]
        public ActionResult<IEnumerable<CinemaDTO.JobEmployeeQualificationMapDTO>> JobEmployeeQualification()
        {
            return Ok(_jqs.GetJobQualifications());
        }


        /// <summary>
        /// crea un nuono lavoro per i soli dipendenti
        /// Le qualifiche devono essere univoche (non ci possono essere 2 qualifiche “Responsabile di sala” per es.) 
        /// create new employ
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>        
        [HttpPost("CreateNewJob"), Authorize(Roles = "ADMIN")]
        public ActionResult<string> CreateNewJob([FromBody] CinemaDTO.JobEmployeeQualificationMinimalDTO job)
        {
            //if (job.Description == null || job.ShortDescr == null)
            //{
            //    return BadRequest();
            //}

            var r = _jqs.CreateNewJob(job);
            switch (r)
            {
                case JobQualificationService.JobQualificationServiceEnum.CREATED:
                case JobQualificationService.JobQualificationServiceEnum.ALREADY_EXISTS:
                    return Ok(r.ToString());
                default:
                    return NotFound(r.ToString());
            }

        }


        [HttpPatch, Authorize(Roles = "ADMIN")]
        public ActionResult<string> UpdateJobEmployeeQualification([FromBody] CinemaDTO.JobEmployeeQualificationMapDTO job)
        {
            var r = _jqs.UpdateJobEmployeeQualification(job);
            return Ok(r.ToString());
        }

        [HttpDelete(Name = "DeleteJobEmployeeQualification"), Authorize(Roles = "ADMIN")]
        public ActionResult<string> DeleteJobEmployeeQualification([FromQuery] int idJobEmp)
        {
            var r = _jqs.DeleteJobEmployeeQualification(idJobEmp);
            switch (r)
            {
                case JobQualificationService.JobQualificationServiceEnum.DELETED:
                    return Ok(r.ToString());
                case JobQualificationService.JobQualificationServiceEnum.NOT_FOUND:
                    return NotFound(r.ToString());
                default:
                    return BadRequest(r.ToString());
            }

        }

    }
}
