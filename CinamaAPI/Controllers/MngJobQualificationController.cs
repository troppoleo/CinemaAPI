using CinemaBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MngJobQualificationController : ControllerBase
    {
        private readonly IJobQualificationService _jqs;

        public MngJobQualificationController(CinemaBL.IJobQualificationService jqs)
        {
            _jqs = jqs;
        }



        /// <summary>
        /// crea un nuono lavoro per i soli dipendenti
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("CreateNewJob")]
        public IActionResult CreateNewJob([FromBody] CinemaDTO.JobDTO job)
        {
            if (job.Description == null || job.ShortDescr == null)
            {
                return BadRequest();
            }
            _jqs.CreateNewJob(job);
            return Ok();
        }
    }
}
