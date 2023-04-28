using CinemaBL;
using CinemaDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobQualificationController : ControllerBase
    {
        private readonly IJobQualificationService _jqs;

        public JobQualificationController(CinemaBL.IJobQualificationService jqs)
        {
            _jqs = jqs;
        }


        //[HttpGet(Name = "GetJobQualification")]
        //public IEnumerable<string> GetJobQualifications()
        //{ 
        //    return _jqs.GetJobQualifications();
        //}

    }
}
