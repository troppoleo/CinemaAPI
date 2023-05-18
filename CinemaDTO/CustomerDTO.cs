using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaDTO
{
    public class CustomerForInsertDTO
    {

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public DateTime Birthdate { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string UserName { get; set; } = null!;

    }
    public class CustomerDTO : CustomerForInsertDTO
    {
        public int Id { get; set; }
    }
}
