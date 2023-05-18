using CinemaBL.Extension;
using CinemaBL.Paging;
using CinemaBL.Repository;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface IReviewService
    {
        IEnumerable<RateReviewDTO> GetReview(PaginationFilter pf);
    }

    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWorkGeneric _uow;

        public ReviewService(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }

        public IEnumerable<RateReviewDTO> GetReview(PaginationFilter pf)
        {
            
                var ll = _uow.GetViewReviewRep.GetPage(pf).Select(x => new RateReviewDTO()
                {
                    CommentNote = x.CommentNote,
                    FilmName = x.FilmName,
                    actorRate = x.ActorRate.ToDefault(),
                    ambientRate = x.AmbientRate.ToDefault(),
                    tramaRate= x.TramaRate.ToDefault()
                });
            
            return ll;
        }
    }
}
