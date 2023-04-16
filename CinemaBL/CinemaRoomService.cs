using CinemaDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface ICinemaRoomService
    {
        List<CinemaRoom> GetCinemaRooms();
    }

    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly CinemaContext _ctx;

        public CinemaRoomService(CinemaDAL.Models.CinemaContext ctx)
        {
            _ctx = ctx;
        }
        public List<CinemaDAL.Models.CinemaRoom> GetCinemaRooms()
        {
            return _ctx.CinemaRooms.ToList();
        }
    }
}
