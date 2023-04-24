using AutoMapper;
using CinemaDAL.Models;
using CinemaDTO;

namespace CinemaBL
{
    public interface IJobQualificationService
    {
        /// <summary>
        /// crea un nuovo JobQualification
        /// </summary>
        void CreateNewJob(CinemaDTO.JobDTO job);
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

        public void CreateNewJob(CinemaDTO.JobDTO job)
        {
            JobQualification j = new JobQualification();
            j.Description = job.Description;
            j.ShortDescr = job.ShortDescr;

            _ctx.JobQualifications.Add(j);
        }

        public List<string> GetJobQualifications()
        {
            var jq = _ctx.JobQualifications.Select(x => x.Description).ToList();
            //var dtoJQ = _mp.Map<List<JobQualificationDTO>>(jq);

            return jq;
        }
    }
}