using AutoMapper.Internal.Mappers;
using CinemaDAL.Models;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static CinemaBL.JobQualificationService;
using static CinemaBL.UsersMng;

namespace CinemaBL
{
    public interface IUsersMng
    {
        UserModel? Autheticate(LoginModel loginModel);
        UsersMngEnum CreateEmployee(CinemaDTO.UsersEmployeeMinimalDTO emp);
        UsersMngEnum UpdateEmployee(UsersEmployeeDTO emp);
        IEnumerable<UsersEmployeeDTO> GetUsersEmployee();
        IEnumerable<UsersEmployeeDTO> GetUsersEmployeeByUserName(string pUserName);
        UsersMngEnum UpdateEmployeeJob(UsersEmployeeJobDTO emp);
        UsersMngEnum DeleteEmployeeJob(int id);
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
            /// Cosa fa
            /// cerca nella varie tabelle di che tipo di utente si tratta
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
                    userModel.Birthdate = em.Birthdate.Value;
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



        public enum UsersMngEnum
        {
            CREATED,
            DELETED,
            INSERTED,
            UPDATED,
            ALREADY_EXISTS,
            JOB_QUALIFICATION_NOT_EXISTS,
            NONE,
            NOT_FOUND,
            /// <summary>
            /// violazione di un requisito minimo
            /// </summary>
            VIOLATION_MINIMUM_REQUIRED
        }


        /// <summary>
        /// crea un nuovo emply
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public UsersMngEnum CreateEmployee(CinemaDTO.UsersEmployeeMinimalDTO emp)
        {
            /// COSA FA:
            /// verifica se ci sono i valori minimi necessari per l'inserimento di un employee
            /// controlla che non sia già censita la userName
            /// inserisce l'employee
            /// 

            if (emp.JobQualificationId.Value == 0)
            {
                throw new ArgumentNullException($"{nameof(emp.JobQualificationId)} is null");
            }

            if (string.IsNullOrWhiteSpace(emp.UserName))
            {
                throw new ArgumentNullException($"{nameof(emp.UserName)} is null");
            }

            if (string.IsNullOrWhiteSpace(emp.Password))
            {
                throw new ArgumentNullException($"{nameof(emp.Password)} is null");
            }

            if (_ctx.UsersEmployees.Any(x => x.UserName == emp.UserName))
            {
                return UsersMngEnum.ALREADY_EXISTS;
            }

            if (!_ctx.JobEmployeeQualifications.Any(x => x.Id == emp.JobQualificationId))
            {
                return UsersMngEnum.JOB_QUALIFICATION_NOT_EXISTS;
            }

            _ctx.UsersEmployees.Add(
                new UsersEmployee()
                {
                    UserName = emp.UserName,
                    Password = emp.Password,
                    //Birthdate = emp.Birthdate,
                    //Name = emp.Name,
                    //Surname = emp.Surname,
                    JobQualificationId = emp.JobQualificationId.Value
                });

            return UsersMngEnum.CREATED;
        }


        public UsersMngEnum UpdateEmployee(UsersEmployeeDTO emp)
        {
            /// COSA FA
            /// controllo l'esistenza del Employee
            /// aggiorno i dati
            var j = _ctx.UsersEmployees.Where(x => x.Id == emp.Id).FirstOrDefault();

            if (j is null)
            {
                return UsersMngEnum.NOT_FOUND;
            }

            j.UserName = emp.UserName;
            j.Name = emp.Name;
            j.Surname = emp.Surname;
            j.Birthdate = emp.Birthdate;

            var job = new UsersEmployeeJobDTO()
            {
                Id = emp.Id,
                JobQualificationId = emp.JobQualificationId
            };

            j.Password = emp.Password;
            j.IsActive = emp.isActive;
            j.CinemaRoomId = emp.cinemaRoomId;

            var uej = UpdateEmployeeJob(job);
            if (uej == UsersMngEnum.UPDATED)
            {
                _ctx.Entry(j).State = EntityState.Modified;

                return UsersMngEnum.UPDATED;
            }

            return uej;
        }


        /// <summary>
        /// restituisce la lista degli Emplyee
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsersEmployeeDTO> GetUsersEmployee()
        {

            var ll = _ctx.UsersEmployees.Select(x => new UsersEmployeeDTO()
            {
                JobQualificationId = x.JobQualificationId,
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                UserName = x.UserName
            });

            return ll;
        }


        public IEnumerable<UsersEmployeeDTO> GetUsersEmployeeByUserName(string pUserName)
        {

            var ll = _ctx.UsersEmployees.Where(x => x.UserName.ToLower() == pUserName.ToLower()).Select(x => new UsersEmployeeDTO()
            {
                JobQualificationId = x.JobQualificationId,
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                UserName = x.UserName,

            });

            return ll;
        }

        public UsersMngEnum UpdateEmployeeJob(UsersEmployeeJobDTO emp)
        {
            var usr = _ctx.UsersEmployees.Where(x => x.Id == emp.Id).FirstOrDefault();
            if (usr is null)
            {
                return UsersMngEnum.NOT_FOUND;
            }


            //todo: controllo:
            ///•	Per i Responsabili di Sala, quando questi non sono assegnati a nessuna sala 
            if (usr.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.OWN_SALA)
            {
                if (usr.CinemaRoomId is null)
                {
                    return Update(usr);
                }
                else
                {
                    return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;
                }
            }


            ///•	Per i Bigliettai, se ce ne sono almeno due attivi(in modo da rimanere “coperti”)
            if (usr.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.GET_TICKET)
            {
                if (_ctx.UsersEmployees.Count(x => x.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.GET_TICKET)
                    > JobEmployeeQualificationService.GetInstance().MinumRequired)
                {
                    return Update(usr);
                }
            }
            else
            {
                return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;
            }


            return UsersMngEnum.UPDATED;
        }

        private UsersMngEnum Update(UsersEmployee? usr)
        {
            _ctx.Entry(usr).State = EntityState.Modified;
            return UsersMngEnum.UPDATED;
        }

        public UsersMngEnum DeleteEmployeeJob(int id)
        {
            var usr = _ctx.UsersEmployees.Where(x => x.Id == id).FirstOrDefault();

            if (usr == null)
            {
                return UsersMngEnum.NOT_FOUND;
            }

            // posso eliminare l'utente se NON ha una sala associata
            if (_ctx.CinemaRooms is null)
            {                
                _ctx.Entry(usr).State = EntityState.Deleted;
                return UsersMngEnum.DELETED;
            }

            // posso eliminare l'utente se ne sono almeno altri 2 sulla stessa sala
            if (_ctx.UsersEmployees.Count(x => x.CinemaRoomId == usr.CinemaRoomId) > 2)
            {
                _ctx.Entry(usr).State = EntityState.Deleted;
                return UsersMngEnum.DELETED;
            }

            return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;            
        }
    }
}
