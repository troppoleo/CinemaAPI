using CinemaBL.Enums;
using CinemaBL.Repository;
using CinemaBL.Utility;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{

    public interface IUserEmployeeService
    {
        //void AddMinimal(UsersEmployeeMinimalDTO eee);

        CrudCinemaEnum Update(UserEmployeeDTO ue);
        //CinemaEnum AddMinimal(UsersEmployeeMinimalDTO ue);
        IEnumerable<UserEmployeeDTO>? GetAll();
        UserEmployeeDTO? Get(int id);       
        CrudCinemaEnum Insert(UserEmployeeForInsertDTO ued);
        CrudCinemaEnum Delete(int id);
        //void Update(UsersEmployeeDTO ued);
    }

    public class UserEmployeeService : IUserEmployeeService
    {
        private readonly IUnitOfWorkGeneric _uow;
        private readonly IUserUtility _userUtility;

        public UserEmployeeService(IUnitOfWorkGeneric uow, IUserUtility userUtility)
        {
            _uow = uow;
            _userUtility = userUtility;
        }

        public void AddMinimal(UserEmployeeMinimalDTO eee)
        {
            _uow.GetUserEmployeeRep.AddMinimal(eee);
        }

        public UserEmployeeDTO? Get(int id)
        {
            var xx = _uow.GetUserEmployeeRep.Get(x => x.Id == id);
            if (xx.Any())
            {
                return _uow.GetUserEmployeeRep.Get(x => x.Id == id).Select(FillDTO()).FirstOrDefault();
            }
            return null;
        }

        public IEnumerable<UserEmployeeDTO>? GetAll()
        {
            var xx = _uow.UserEmployeeRepository.Get();
            if (xx.Any())
            {
                return xx.Select(FillDTO());
            }
            return null;
        }

        private static Func<UserEmployee, UserEmployeeDTO> FillDTO()
        {
            return x => new UserEmployeeDTO()
            {
                UserName = x.UserName,
                JobQualificationId = x.JobQualificationId,
                Name = x.Name,
                Surname = x.Surname,
                isActive = x.IsActive.Value
            };
        }

        public CrudCinemaEnum Delete(int id)
        {
            var item = _uow.GetUserEmployeeRep.Get(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                var jobEmplQual = _uow.GetJobEmployeeQualificationRep.Get(x => x.Id == item.JobQualificationId).FirstOrDefault();
                if (jobEmplQual is not null && jobEmplQual.MinimumRequired.HasValue)
                {
                    if (_uow.GetUserEmployeeRep.Get(x => x.JobQualificationId == item.JobQualificationId).Count() >= jobEmplQual.MinimumRequired.Value)
                    {
                        _uow.GetUserEmployeeRep.Delete(item);
                        return CrudCinemaEnum.DELETED;
                    }

                    if (jobEmplQual is not null && jobEmplQual.Id == (int)JobEmployeeQualificationEnum.OWN_SALA)
                    {
                        if (!_uow.GetCinemaRoomCrossUserEmployeeRep.Get(x => x.UserEmployeeId == item.Id).Any())
                        {
                            _uow.GetUserEmployeeRep.Delete(item);
                            return CrudCinemaEnum.DELETED;
                        }
                    }

                    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
                }
            }
            return CrudCinemaEnum.NOT_FOUND;
        }

        public CrudCinemaEnum Update(UserEmployeeDTO ue)
        {
            var item = _uow.GetUserEmployeeRep.Get(x => x.Id == ue.Id).FirstOrDefault();

            if (item != null)
            {
                var jobEmplQual = _uow.GetJobEmployeeQualificationRep.Get(x => x.Id == item.JobQualificationId).FirstOrDefault();
                if (jobEmplQual is not null && jobEmplQual.MinimumRequired.HasValue)
                {
                    item.UserName = ue.UserName;
                    item.Password = ue.Password;
                    item.JobQualificationId = ue.JobQualificationId;
                    item.Name = ue.Name;
                    item.Surname = ue.Surname;
                    item.IsActive = ue.isActive;

                    if (_uow.GetUserEmployeeRep.Get(x => x.JobQualificationId == item.JobQualificationId).Count() >= jobEmplQual.MinimumRequired.Value)
                    {
                        _uow.GetUserEmployeeRep.Update(item);
                        return CrudCinemaEnum.UPDATED;
                    }

                    if (jobEmplQual is not null && jobEmplQual.Id == (int)JobEmployeeQualificationEnum.OWN_SALA)
                    {
                        if (!_uow.GetCinemaRoomCrossUserEmployeeRep.Get(x => x.UserEmployeeId == item.Id).Any())
                        {
                            _uow.GetUserEmployeeRep.Update(item);
                            return CrudCinemaEnum.UPDATED;
                        }
                    }

                    return CrudCinemaEnum.VIOLATION_MINIMUM_REQUIRED;
                }
            }

            return CrudCinemaEnum.NOT_FOUND;
        }

        public CrudCinemaEnum Insert(UserEmployeeForInsertDTO ued)
        {
            if (_userUtility.IsUsernameAlreadyUsed(ued.UserName))
            {
                return CrudCinemaEnum.ALREADY_EXISTS;
            }

            _uow.GetUserEmployeeRep.Insert(new UserEmployee()
            {
                Name = ued.Name,
                Surname = ued.Surname,
                UserName = ued.UserName,
                Password = ued.Password,
                JobQualificationId = ued.JobQualificationId,
                IsActive = ued.isActive
            });

            return CrudCinemaEnum.CREATED;


            //var emp = _uow.GetUserEmployeeRep.Get(x => x.UserName.Trim().ToLower() == ued.UserName.Trim().ToLower());
            //if (!emp.Any())
            //{
            //    _uow.GetUserEmployeeRep.Insert(new UserEmployee()
            //    {
            //        Name = ued.Name,
            //        Surname = ued.Surname,
            //        UserName = ued.UserName,
            //        Password = ued.Password,
            //        JobQualificationId = ued.JobQualificationId,
            //        IsActive = ued.isActive
            //    });

            //    return CrudCinemaEnum.CREATED;
            //}

            //return CrudCinemaEnum.ALREADY_EXISTS;
        }

      



        //public void Update(UsersEmployeeDTO ue)
        //{
        //    if (_uow.UserEmployeeExt.Get(x => x.id == ue.Id) == null)
        //    {
        //        return;
        //    }

        //    UsersEmployee usersEmployee = new UsersEmployee()
        //    {
        //        Id = ue.Id,
        //        UserName = ue.UserName,
        //        Password = ue.Password,
        //        JobQualificationId = ue.JobQualificationId.Value,
        //        Name = ue.Name,
        //        Surname = ue.Surname,
        //        Birthdate = ue.Birthdate,
        //        CinemaRoomId = ue.cinemaRoomId,
        //        IsActive = ue.isActive
        //    };

        //    _uow.UserEmployeeExt.Update(ue);
        //}
    }

}
