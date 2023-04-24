using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class UserModel
    {
        public enum UserModelType
        {
            ADMIN,
            EMPLOYEE,
            CUSTOMER
        }

        public string? Name { get; set; }
        public string? JobQualification { get; set; }
        public DateTime Birthdate { get; set; }
        public UserModelType UserType { get; set; }
    }
}
