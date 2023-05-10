using CinemaBL.Enums;
using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.UsersMng;

namespace CinemaBL.Repository
{
    public interface IUsersEmployeeRepository<T> : IRepository<UserEmployee>
    {
        //CinemaEnum AddMinimal(UsersEmployeeMinimalDTO emp);
    }

    public class UsersEmployeeRepository : Repository<UserEmployee>, IUsersEmployeeRepository<UserEmployee>
    {
        
        public UsersEmployeeRepository(CinemaContext ctx) : base(ctx)
        {            
        
        }

        //public CinemaEnum AddMinimal(UsersEmployeeMinimalDTO emp)
        //{
            
        //    if (emp.JobQualificationId.Value == 0)
        //    {
        //        throw new ArgumentNullException($"{nameof(emp.JobQualificationId)} is null");
        //    }

        //    if (string.IsNullOrWhiteSpace(emp.UserName))
        //    {
        //        throw new ArgumentNullException($"{nameof(emp.UserName)} is null");
        //    }

        //    if (string.IsNullOrWhiteSpace(emp.Password))
        //    {
        //        throw new ArgumentNullException($"{nameof(emp.Password)} is null");
        //    }

        //    if (_ctx.UserEmployees.Any(x => x.UserName == emp.UserName))
        //    {
        //        return CinemaEnum.ALREADY_EXISTS;
        //    }

        //    if (!_ctx.JobEmployeeQualifications.Any(x => x.Id == emp.JobQualificationId))
        //    {
        //        return CinemaEnum.JOB_QUALIFICATION_NOT_EXISTS;
        //    }

        //    base._ctx.UserEmployees.Add(
        //        new UserEmployee()
        //        {
        //            UserName = emp.UserName,
        //            Password = emp.Password,
        //            JobQualificationId = emp.JobQualificationId.Value
        //        });

        //    return CinemaEnum.CREATED;

        //}
    }

}
