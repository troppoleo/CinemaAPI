using CinemaBL;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _cs;

        public CustomerController(ICustomerService cs)
        {
            _cs = cs;
        }

        [HttpPost]
        [Route("InsertNewCustomer")]
        public IActionResult InsertNewCustomer(CustomerForInsertDTO cus)
        {
            return Ok(_cs.Insert(cus).ToString());
        }


        [HttpGet]
        [Route("GetMoviesScheduled")]
        public ActionResult<IEnumerable<MovieScheduleForCustomerDTO>> GetMoviesScheduled([FromQuery] MovieFilterForCustomerDTO ff)  // non si può prendere dal body
        {
            return Ok(_cs.GetMoviesScheduled(ff));
        }

        //[HttpGet]
        //[Route("Get/{genere}/{startDate}")]
        //public IActionResult Get(string genere, DateTime startDate)
        //{
        //    return Ok(genere);
        //}

        //[HttpGet]
        //[Route("Get")]
        //public IActionResult Get([FromQuery] MovieFilterForCustomerDTO ff)
        //{
        //    return Ok(ff.Genere);
        //}




    }
}
