using CinemaBL;
using CinemaBL.Enums;
using CinemaBL.Extension;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;

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

        [HttpGet]
        [Route("GetMovieDetail/{idMovieSchedule}")]
        public ActionResult<IEnumerable<MovieDetailForCustomerDTO>> GetMovieDetail(int idMovieSchedule)
        {
            return Ok(_cs.GetMovieDetail(idMovieSchedule));
        }


        /// <summary>
        /// restituisce l'id del ticket creato
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("BuyTickets")]
        [Authorize(Roles = "CUSTOMER")]
        public ActionResult<BuyedTicketsCustomerDTO> BuyTickets([FromBody] BuyTicketCustomerDTO btc)
        {           
            var idCustomer = GetIdCustomerFromClaim();
            if (idCustomer != null)            
            {
                var result = _cs.BuyTickets(btc, idCustomer.Value);
                if (result.MessageBackForCustomer.ToEnum<MessageForUserEnum>() != MessageForUserEnum.DONE)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }


            return BadRequest(new BuyedTicketsCustomerDTO() { MessageBackForCustomer = MessageForUserEnum.ERROR.ToString() });
        }
        private int? GetIdCustomerFromClaim()
        {
            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "Id"))
            {
                return int.Parse(currentUser.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First());
            }
            return null;
        }

        [HttpDelete]
        [Route("DeleteTicket/{ticketId}")]
        [Authorize(Roles = "CUSTOMER")]
        public ActionResult<string> DeleteTicket(int ticketId)
        {
            var idCustomer = GetIdCustomerFromClaim();
            if (idCustomer != null)
            {
                MessageForUserEnum userEnum = _cs.DeleteTicket(ticketId, idCustomer.Value);
                if (userEnum == MessageForUserEnum.DONE)
                {
                    return Ok(userEnum.ToString());
                }
                return BadRequest(userEnum.ToString());
            }

            return BadRequest(MessageForUserEnum.USER_NOT_AUTHORIZED.ToString());
        }

        [HttpPost]
        [Route("InsertMovieReview")]
        [Authorize(Roles = "CUSTOMER")]
        public ActionResult<string> InsertMovieReview([FromBody] MovieReviewDTO mr)
        {
            var idCustomer = GetIdCustomerFromClaim();
            if (idCustomer != null)
            {
                return Ok(_cs.InsertMovieReview(mr, idCustomer.Value).ToString());
            }
            return BadRequest(MessageForUserEnum.USER_NOT_AUTHORIZED.ToString());
        }


        [HttpGet]
        [Route("GetWatchedMovies")]
        [Authorize(Roles = "CUSTOMER")]
        public ActionResult<object> GetWatchedMovies()
        {
            var idCustomer = GetIdCustomerFromClaim();
            if (idCustomer != null)
            {
                return Ok(_cs.GetWatchedMovies(idCustomer.Value));
            }
            return BadRequest(MessageForUserEnum.USER_NOT_AUTHORIZED.ToString());
        }

    }
}
