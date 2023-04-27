using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface IUsersMng
    {
        UserModel? Autheticate(LoginModel loginModel);
    }

    public class UsersMng : IUsersMng
    {
        private readonly CinemaContext _ctx;

        public UsersMng(CinemaDAL.Models.CinemaContext ctx)
        {
            _ctx = ctx;
        }


        public CinemaDTO.UserModel? Autheticate(LoginModel loginModel)
        {
            try
            {
                // verifico se è un ADMIN
                var ua = _ctx.UsersAdmins.Where(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password).FirstOrDefault();

                if (ua is not null)
                {
                    return new CinemaDTO.UserModel()
                    {
                        Name = ua.Name,
                        UserType = UserModel.UserModelType.ADMIN
                    };
                }

                // verifico se è un "Employees"
                var em = _ctx.UsersEmployees
                    .Where(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password)
                    .Include(x => x.JobQualification).FirstOrDefault();
                if (em is not null)
                {
                    UserModel userModel = new UserModel();
                    userModel.UserType = UserModel.UserModelType.EMPLOYEE;
                    userModel.Birthdate = em.Birthdate;
                    userModel.Name = em.Name;
                    userModel.JobQualification = em.JobQualification.ShortDescr;

                    return userModel;
                }

                // TODO: verifico se è un "CUSTOMER"             

                // utente non trovato:
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
