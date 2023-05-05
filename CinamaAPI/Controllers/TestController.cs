using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet(Name = "TestEccezioni")]
        public IActionResult TestEccezioni()
        {
            throw new Exception("Simulazione di eccezione");
        }
    }
}
