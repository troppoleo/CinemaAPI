using CinemaBL;
using CinemaBL.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    /*
    Visualizzare le recensioni fatte dai CUSTOMER (non in diretta, semplicemente un’API che tira su la lista delle recensioni fatte su un film, 
    la lista deve essere paginata e con filtri (numero stelle e/o criterio)
    */
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _rs;

        public ReviewController(IReviewService rs)
        {
            _rs = rs;
        }

        [HttpGet]
        [Route("GetReview")]
        public IActionResult GetReview([FromQuery] PaginationFilter pf)
        {            
            return Ok(_rs.GetReview(pf));

        }
    }
}
