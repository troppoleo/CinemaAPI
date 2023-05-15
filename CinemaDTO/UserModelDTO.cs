using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class UserModelDTO
    {
        public enum UserModelType
        {
            ADMIN,
            EMPLOYEE,
            CUSTOMER
        }

        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? JobQualification { get; set; }
        //public DateTime Birthdate { get; set; }
        public UserModelType UserType { get; set; }
    }
}
