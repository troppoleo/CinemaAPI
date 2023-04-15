using CinemaDAL.Models;
using CinemaDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CinemaBL
{
    public interface IUserTypeService
    {
        List<UserTypeDTO>? GetUserType();
    }

    public class UserTypeService: IUserTypeService
    {
        private readonly CinemaContext _ctx;
        private readonly IMapper _iau;

        public UserTypeService(CinemaDAL.Models.CinemaContext ctx, AutoMapper.IMapper iau)
        {
            _ctx = ctx;
            _iau = iau;
        }


        public List<UserTypeDTO> GetUserType()
        {
            var ut = _ctx.UserTypes.ToList();
            var dtoUT = _iau.Map<List<UserTypeDTO>>(ut);

            return dtoUT;
        }

    }
}
