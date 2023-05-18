using CinemaBL.Repository;
using CinemaDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Utility
{

    /// <summary>
    /// utile per i controlli comuni alle utenze
    /// </summary>
    public interface IUserUtility
    {
        /// <summary>
        /// controlla in tutte la tabelle anagrafiche la presenza della userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsUsernameAlreadyUsed(string userName);
        int GetAge(DateTime dt);
    }

    public class UserUtility : IUserUtility
    {
        private readonly IUnitOfWorkGeneric _uow;

        public UserUtility(IUnitOfWorkGeneric uow)
        {
            _uow = uow;
        }

        public bool IsUsernameAlreadyUsed(string userName)
        {
            string  myUserName = userName.Trim().ToLower();

            if (_uow.GetUserEmployeeRep.Get(x => x.UserName.Trim().ToLower() == myUserName).Any())
            {
                return true;
            }

            if (_uow.GetUsersAdminRep.Get(x => x.UserName.Trim().ToLower() == myUserName).Any())
            {
                return true;
            }

            if (_uow.GetCustomerRep.Get(x => x.UserName.Trim().ToLower() == myUserName).Any())
            {
                return true;
            }

            return false;
        }


        public int GetAge(DateTime dt)
        {
            return  (int)Math.Truncate(DateTime.Now.Subtract(dt).TotalDays / 365);
        }
    }
}
