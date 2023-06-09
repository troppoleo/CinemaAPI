﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{

    public class UserEmployeeMinimalDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int JobQualificationId { get; set; }
    }

    /// <summary>
    /// utile per cambiare solo i JOB
    /// </summary>
    public class UserEmployeeJobDTO
    {
        public int Id { get; set; }
        public int JobQualificationId { get; set; }
    }
    public class UserEmployeeForInsertDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //public DateTime Birthdate { get; set; }
        public int JobQualificationId { get; set; }
        public int isActive { get; set; }
    }
    public class UserEmployeeDTO : UserEmployeeMinimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public DateTime Birthdate { get; set; }
       // public int cinemaRoomId { get; set; }
        public int isActive { get; set; }
    }
}
