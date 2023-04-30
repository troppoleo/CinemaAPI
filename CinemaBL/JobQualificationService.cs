using AutoMapper;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using static CinemaBL.JobQualificationService;

namespace CinemaBL
{
    public interface IJobQualificationService
    {
        JobQualificationServiceEnum CreateNewJob(JobEmployeeQualificationMinimalDTO job);
        JobQualificationServiceEnum DeleteJobEmployeeQualification(int idJobEmp);
        IEnumerable<JobEmployeeQualificationMapDTO> GetJobQualifications();
        JobQualificationServiceEnum UpdateJobEmployeeQualification(JobEmployeeQualificationMapDTO job);
    }

    public class JobQualificationService : IJobQualificationService
    {
        public enum JobQualificationServiceEnum
        {
            CREATED,
            DELETED,
            UPDATED,
            ALREADY_EXISTS,
            NOT_FOUND,
            NOT_REMOVABLE_BECAUSE_HAS_EMPLOY,
            NOT_UPDATABLE_BECAUSE_SHORTDESCR_ALREAY_EXISTS
        }

        private readonly CinemaContext _ctx;
        private readonly IMapper _mp;

        public JobQualificationService(CinemaDAL.Models.CinemaContext ctx, AutoMapper.IMapper mp)
        {
            _ctx = ctx;
            _mp = mp;
        }

        public JobQualificationServiceEnum CreateNewJob(CinemaDTO.JobEmployeeQualificationMinimalDTO job)
        {
            if (_ctx.JobEmployeeQualifications.Any(x => x.ShortDescr == job.ShortDescr))
            {
                return JobQualificationServiceEnum.ALREADY_EXISTS;
            }

            _ctx.JobEmployeeQualifications.Add(
                new JobEmployeeQualification()
                {
                    Description = job.Description,
                    ShortDescr = job.ShortDescr
                });
            return JobQualificationServiceEnum.CREATED;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="idJobEmp"></param>
        /// <returns></returns>
        public JobQualificationServiceEnum DeleteJobEmployeeQualification(int idJobEmp)
        {
            /// COSA FA:
            /// cerco la qualifica
            /// verifico se l'ID fornito esiste
            /// controllo se ha degli impiegati associati ed eventualmente non può essere cancellata
            /// passa tutti i controlli posso cancellare il Job

            //_ctx.JobEmployeeQualifications.Remove(_ctx.JobEmployeeQualifications.Where(x => x.Id == idJobEmp).);
            //_ctx.Entry(new JobEmployeeQualification() { Id = idJobEmp }).State = EntityState.Deleted;            

            var j = _ctx.JobEmployeeQualifications.Where(x => x.Id == idJobEmp).FirstOrDefault();

            if (j is null)
            {
                return JobQualificationServiceEnum.NOT_FOUND;
            }

            /// Non può essere né eliminata né modificata una qualifica se ha anche solo un EMPLOYEE associato 
            if (_ctx.UsersEmployees.Any(x => x.Id == j.Id))
            {
                return JobQualificationServiceEnum.NOT_REMOVABLE_BECAUSE_HAS_EMPLOY;
            }

            _ctx.Entry(j).State = EntityState.Deleted;
            return JobQualificationServiceEnum.DELETED;
        }

        public IEnumerable<CinemaDTO.JobEmployeeQualificationMapDTO> GetJobQualifications()
        {
            /// COSA FA
            /// restituisce la lista delle possibile qualifiche per gli Employe


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

        public JobQualificationServiceEnum UpdateJobEmployeeQualification(JobEmployeeQualificationMapDTO job)
        {
            /// COSA FA
            /// controllo l'esistenza del Job
            /// controllo che il nuovo Job Short Description non sia stato già censito
            /// se passo i controlli
            /// aggiorno i dati
            var j = _ctx.JobEmployeeQualifications.Where(x => x.Id == job.Id).FirstOrDefault();

            if (j is null)
            {
                return JobQualificationServiceEnum.NOT_FOUND;
            }

            if (_ctx.JobEmployeeQualifications.Any(x => x.ShortDescr == job.ShortDescr))
            {
                return JobQualificationServiceEnum.NOT_UPDATABLE_BECAUSE_SHORTDESCR_ALREAY_EXISTS;
            }

            j.Description = job.Description;
            j.ShortDescr = job.ShortDescr;

            //j.ToList().ForEach(x =>
            //{
            //    x.Description = job.Description;
            //    x.ShortDescr = job.ShortDescr;
            //});

            return JobQualificationServiceEnum.UPDATED;
        }
    }
}