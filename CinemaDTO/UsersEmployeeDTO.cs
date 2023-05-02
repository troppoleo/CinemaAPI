using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{

    public class UsersEmployeeMinimalDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? JobQualificationId { get; set; }
    }

    //public class UsersEmployeeUpdateDTO : UsersEmployeeMinimalDTO
    //{
    //    public int Id { get; set; }
    //    public string? Name { get; set; }
    //    public string? Surname { get; set; }
    //    public DateTime Birthdate { get; set; }
    //}


    /// <summary>
    /// utile per cambiare solo i JOB
    /// </summary>
    public class UsersEmployeeJobDTO
    {
        public int Id { get; set; }
        public int? JobQualificationId { get; set; }
    }

    public class UsersEmployeeDTO : UsersEmployeeMinimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public int JobQualificationID { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        public DateTime Birthdate { get; set; }

        public int cinemaRoomId { get; set; }
        public int isActive { get; set; }
    }
}
