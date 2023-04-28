using AutoMapper;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;

namespace CinemaBL
{
    public interface IJobQualificationService
    {
        void CreateNewJob(JobEmployeeQualificationMinimalDTO job);
        bool DeleteJobEmployeeQualification(int idJobEmp);
        IEnumerable<JobEmployeeQualificationMapDTO> GetJobQualifications();
        bool UpdateJobEmployeeQualification(JobEmployeeQualificationMapDTO job);
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

        public void CreateNewJob(CinemaDTO.JobEmployeeQualificationMinimalDTO job)
        {
            _ctx.JobEmployeeQualifications.Add(
                new JobEmployeeQualification()
                {
                    Description = job.Description,
                    ShortDescr = job.ShortDescr
                });
        }

        public bool DeleteJobEmployeeQualification(int idJobEmp)
        {
            //_ctx.JobEmployeeQualifications.Remove(_ctx.JobEmployeeQualifications.Where(x => x.Id == idJobEmp).);
            //_ctx.Entry(new JobEmployeeQualification() { Id = idJobEmp }).State = EntityState.Deleted;            

            var j = _ctx.JobEmployeeQualifications.Where(x => x.Id == idJobEmp);

            if (j is null || j.Count() == 0)
            {
                // not found
                return false;
            }

            _ctx.Entry(j.First()).State = EntityState.Deleted;
            return true;
        }

        public IEnumerable<CinemaDTO.JobEmployeeQualificationMapDTO> GetJobQualifications()
        {
            //var dtoJQ = _mp.Map<List<JobQualificationDTO>>(jq);

            int i = _ctx.JobEmployeeQualifications.Count();

            var ll = _ctx.JobEmployeeQualifications.Select(x => new JobEmployeeQualificationMapDTO()
            {
                Description = x.Description,
                Id = x.Id,
                ShortDescr = x.ShortDescr
            });


            return ll;
        }

        public bool UpdateJobEmployeeQualification(JobEmployeeQualificationMapDTO job)
        {
            var j = _ctx.JobEmployeeQualifications.Where(x => x.Id == job.Id);

            if (j is null || j.Count() == 0)
            {
                // not found
                return false;
            }

            j.ToList().ForEach(x =>
            {
                x.Description = job.Description;
                x.ShortDescr = job.ShortDescr;
            });

            return true;
        }
    }
}