using CinemaBL;
using CinemaBL.Mapper;
using CinemaDTO;
using Microsoft.AspNetCore.Mvc;

namespace CinamaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTypesController : ControllerBase   //usare ControllerBase  per web API
    {
        private readonly IUserTypeService _uts;

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public UserTypesController(CinemaBL.IUserTypeService uts)
        {
            _uts = uts;
        }

        [HttpGet(Name = "GetUserTypes")]
        public IEnumerable<UserTypeDTO>? GetUserTypes()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            return _uts.GetUserType();
        }
    }
}
