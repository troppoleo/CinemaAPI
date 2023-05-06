using CinemaDAL.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL.Repository
{
    public interface ICinemaRoomRepository
    {
    }

    public class CinemaRoomRepository : Repository<CinemaDAL.Models.CinemaRoom>, ICinemaRoomRepository
    {
        public CinemaRoomRepository(CinemaContext ctx) : base(ctx)
        {

        }     
    }
}
