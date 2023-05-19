using CinemaBL.Enums;
using CinemaBL.Repository;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface IJobEmployeeQualificationService
    {
        IEnumerable<JobEmployeeQualificationDTO> GetAll();
        Enums.CrudCinemaEnum Delete(int id);
        JobEmployeeQualificationDTO GetByID(int id);
        CrudCinemaEnum Update(JobEmployeeQualificationDTO jeq);
        CrudCinemaEnum Insert(JobEmployeeQualificationForInsertDTO jeq);

        //int MinumRequired { get; }
    }

    public class JobEmployeeQualificationService : IJobEmployeeQualificationService
    {
        //private static JobEmployeeQualificationService _instance;
        private readonly IUnitOfWorkGeneric _uow;

        public JobEmployeeQualificationService(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }

        public IEnumerable<JobEmployeeQualificationDTO> GetAll()
        {
            return _uow.GetJobEmployeeQualificationRep.Get().Select(x => new JobEmployeeQualificationDTO()
            {
                Id= x.Id,
                ShortDescr = x.ShortDescr,
                Description = x.Description,
                MinimumRequired = x.MinimumRequired
            });
        }

        public CrudCinemaEnum Delete(int id)
        {            
            // controlla che non ci siano employee con la qualifica che da eliminare            
            if (_uow.GetUserEmployeeRep.Get(x => x.JobQualificationId == id).Any())
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }

            var itemToDelete = _uow.GetJobEmployeeQualificationRep.GetByID(id);

            if (itemToDelete != null)
            {
                _uow.GetJobEmployeeQualificationRep.Delete(itemToDelete);
                return CrudCinemaEnum.DELETED;
            }
            return CrudCinemaEnum.NOT_FOUND;
        }

        public JobEmployeeQualificationDTO GetByID(int id)
        {
            var item = _uow.GetJobEmployeeQualificationRep.Get(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                return new JobEmployeeQualificationDTO()
                {
                    Id = item.Id,
                    Description = item.Description,
                    ShortDescr = item.ShortDescr,
                    MinimumRequired = item.MinimumRequired
                };
            }
            return null;
        }

        public CrudCinemaEnum Update(JobEmployeeQualificationDTO jeq)
        {
            // controlla che non ci siano employee con la qualifica che da modificare
            var empl = _uow.GetUserEmployeeRep.Get(x => x.JobQualificationId == jeq.Id);
            if (empl != null)
            {
                return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
            }


            var item = _uow.GetJobEmployeeQualificationRep.Get(x => x.Id == jeq.Id).FirstOrDefault();
            if (item != null)
            {
                item.ShortDescr = jeq.ShortDescr;
                item.Description = jeq.Description;
                item.MinimumRequired = jeq.MinimumRequired;

                return CrudCinemaEnum.UPDATED;
            }

            return CrudCinemaEnum.NOT_FOUND;
        }

        public CrudCinemaEnum Insert(JobEmployeeQualificationForInsertDTO jeq)
        {
            // controllo se la qualifica esite già
            var item = _uow.GetJobEmployeeQualificationRep.Get(x => x.ShortDescr.ToLower().Trim() == jeq.ShortDescr.ToLower().Trim()).FirstOrDefault();
            if (item == null)
            {
                _uow.GetJobEmployeeQualificationRep.Insert(new JobEmployeeQualification()
                {
                    Description = jeq.Description,
                    ShortDescr = jeq.ShortDescr,
                    MinimumRequired = jeq.MinimumRequired
                });
                return CrudCinemaEnum.CREATED;
            }

            return CrudCinemaEnum.ALREADY_EXISTS;
        }

        //public int MinumRequired { get; private set; }

        //public static JobEmployeeQualificationService GetInstance()
        //{
        //    if (_instance == null)
        //    {
        //        _instance = new JobEmployeeQualificationService();

        //        CinemaContext ctx = new CinemaContext();

        //        // TODO: "OWN_SALA" andrebbe preso da file di configurazione
        //        var own_sala = ctx.JobEmployeeQualifications.Where(x => x.ShortDescr == JobEmployeeQualificationEnum.OWN_SALA.ToString());
        //        _instance.MinumRequired = own_sala.Select(x => x.MinimumRequired).First().Value;
        //    }
        //    return _instance;
        //}

        /// <summary>
        /// contiene le mansioni fisse
        /// </summary>
        //public enum JobEmployeeQualificationEnum
        //{
        //    /// <summary>
        //    /// (bigliettaio) fornitore di biglietti
        //    /// </summary>
        //    GET_TICKET = 1,
        //    /// <summary>
        //    /// Responsabile di sala
        //    /// </summary>
        //    OWN_SALA = 2
        //}
    }
}
