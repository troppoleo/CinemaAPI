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
        UserModelDTO? Autheticate(LoginModelDTO loginModel);
        UsersMngEnum CreateEmployee(UserEmployeeMinimalDTO emp);
        UsersMngEnum UpdateEmployee(UserEmployeeDTO emp);
        IEnumerable<UserEmployeeDTO> GetUsersEmployee();
        IEnumerable<UserEmployeeDTO> GetUsersEmployeeByUserName(string pUserName);
        UsersMngEnum UpdateEmployeeJob(UserEmployeeJobDTO emp);
        UsersMngEnum DeleteEmployeeJob(int id);
    }

    public class UsersMng : IUsersMng
    {
        private readonly CinemaContext _ctx;

        public UsersMng(CinemaDAL.Models.CinemaContext ctx)
        {
            _ctx = ctx;
        }


        public CinemaDTO.UserModelDTO? Autheticate(LoginModelDTO loginModel)
        {
            /// Cosa fa
            /// cerca nella varie tabelle di che tipo di utente si tratta

            // verifico se è un ADMIN
            var ua = _ctx.UsersAdmins.Where(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password).FirstOrDefault();

            if (ua is not null)
            {
                return new UserModelDTO()
                {
                    UserName = ua.UserName,
                    UserType = UserModelDTO.UserModelType.ADMIN,
                    Id = ua.Id
                };
            }

            // verifico se è un "Employees"
            var em = _ctx.UserEmployees
                .Where(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password)
                .Include(x => x.JobQualification).FirstOrDefault();
            if (em is not null)
            {
                return new UserModelDTO()
                {
                    UserType = UserModelDTO.UserModelType.EMPLOYEE,
                    UserName = em.UserName,
                    JobQualification = em.JobQualification.ShortDescr,
                    Id = em.Id
                };
            }

            // TODO: verifico se è un "CUSTOMER"             

            // utente non trovato:
            return null;
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
        /// crea un nuovo employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [Obsolete("Use instead Add of Repository", DiagnosticId = "Metodo Obsoleto")]
        public UsersMngEnum CreateEmployee(CinemaDTO.UserEmployeeMinimalDTO emp)
        {
            /// COSA FA:
            /// verifica se ci sono i valori minimi necessari per l'inserimento di un employee
            /// controlla che non sia già censita la userName
            /// inserisce l'employee
            ///             

            if (string.IsNullOrWhiteSpace(emp.UserName))
            {
                throw new ArgumentNullException($"{nameof(emp.UserName)} is null");
            }

            if (string.IsNullOrWhiteSpace(emp.Password))
            {
                throw new ArgumentNullException($"{nameof(emp.Password)} is null");
            }

            if (_ctx.UserEmployees.Any(x => x.UserName == emp.UserName))
            {
                return UsersMngEnum.ALREADY_EXISTS;
            }

            if (!_ctx.JobEmployeeQualifications.Any(x => x.Id == emp.JobQualificationId))
            {
                return UsersMngEnum.JOB_QUALIFICATION_NOT_EXISTS;
            }

            _ctx.UserEmployees.Add(
                new UserEmployee()
                {
                    UserName = emp.UserName,
                    Password = emp.Password,
                    //Birthdate = emp.Birthdate,
                    //Name = emp.Name,
                    //Surname = emp.Surname,
                    JobQualificationId = emp.JobQualificationId
                });

            return UsersMngEnum.CREATED;
        }


        public UsersMngEnum UpdateEmployee(UserEmployeeDTO emp)
        {
            /// COSA FA
            /// controllo l'esistenza del Employee
            /// aggiorno i dati
            var j = _ctx.UserEmployees.Where(x => x.Id == emp.Id).FirstOrDefault();

            if (j is null)
            {
                return UsersMngEnum.NOT_FOUND;
            }

            j.UserName = emp.UserName;
            j.Name = emp.Name;
            j.Surname = emp.Surname;

            var job = new UserEmployeeJobDTO()
            {
                Id = emp.Id,
                JobQualificationId = emp.JobQualificationId
            };

            j.Password = emp.Password;
            j.IsActive = emp.isActive;
            //j.CinemaRoomId = emp.cinemaRoomId;

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
        public IEnumerable<UserEmployeeDTO> GetUsersEmployee()
        {

            var ll = _ctx.UserEmployees.Select(x => new UserEmployeeDTO()
            {
                JobQualificationId = x.JobQualificationId,
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                UserName = x.UserName
            });

            return ll;
        }


        public IEnumerable<UserEmployeeDTO> GetUsersEmployeeByUserName(string pUserName)
        {

            var ll = _ctx.UserEmployees.Where(x => x.UserName.ToLower() == pUserName.ToLower()).Select(x => new UserEmployeeDTO()
            {
                JobQualificationId = x.JobQualificationId,
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                UserName = x.UserName,

            });

            return ll;
        }

        public UsersMngEnum UpdateEmployeeJob(UserEmployeeJobDTO emp)
        {
            var usr = _ctx.UserEmployees.Where(x => x.Id == emp.Id).FirstOrDefault();
            if (usr is null)
            {
                return UsersMngEnum.NOT_FOUND;
            }


            //todo: controllo:
            ///•	Per i Responsabili di Sala, quando questi non sono assegnati a nessuna sala 
            //if (usr.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.OWN_SALA)
            //{
            //    if (usr.CinemaRoomId is null)
            //    {
            //        return Update(usr);
            //    }
            //    else
            //    {
            //        return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;
            //    }
            //}


            ///•	Per i Bigliettai, se ce ne sono almeno due attivi(in modo da rimanere “coperti”)
            //if (usr.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.GET_TICKET)
            //{
            //    if (_ctx.UserEmployees.Count(x => x.JobQualificationId == (int)JobEmployeeQualificationService.JobEmployeeQualificationEnum.GET_TICKET)
            //        > JobEmployeeQualificationService.GetInstance().MinumRequired)
            //    {
            //        return Update(usr);
            //    }
            //}
            //else
            //{
            //    return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;
            //}


            return UsersMngEnum.UPDATED;
        }

        private UsersMngEnum Update(UserEmployee usr)
        {
            _ctx.Entry(usr).State = EntityState.Modified;
            return UsersMngEnum.UPDATED;
        }

        public UsersMngEnum DeleteEmployeeJob(int id)
        {
            var usr = _ctx.UserEmployees.Where(x => x.Id == id).FirstOrDefault();

            if (usr == null)
            {
                return UsersMngEnum.NOT_FOUND;
            }

            // posso eliminare l'utente se ne sono almeno altri 2 sulla stessa sala
            //if (_ctx.UserEmployees.Count(x => x.CinemaRoomId == usr.CinemaRoomId) > 2)
            //{
            //    _ctx.Entry(usr).State = EntityState.Deleted;
            //    return UsersMngEnum.DELETED;
            //}

            // posso eliminare l'utente se NON ha una sala associata
            if (_ctx.CinemaRooms is null)
            {
                _ctx.Entry(usr).State = EntityState.Deleted;
                return UsersMngEnum.DELETED;
            }

            return UsersMngEnum.VIOLATION_MINIMUM_REQUIRED;
        }
    }
}
