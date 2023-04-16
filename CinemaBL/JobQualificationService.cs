using AutoMapper;
using CinemaDAL.Models;
using CinemaDTO;

namespace CinemaBL
{
    public interface IJobQualificationService
    {
        List<string>? GetJobQualifications();
    }

    public class JobQualificationService : IJobQualificationService
    {
        private readonly CinemaContext _ctx;
        private readonly IMapper _mp;

        public JobQualificationService(CinemaDAL.Models.CinemaContext ctx, AutoMapper.IMapper mp)
        {
            _ctx = ctx;
            _mp = mp;
        }        

        public List<string> GetJobQualifications()
        {
            var jq = _ctx.JobQualifications.Select(x=> x.Description).ToList();
            //var dtoJQ = _mp.Map<List<JobQualificationDTO>>(jq);

            return jq;
        }
    }
}