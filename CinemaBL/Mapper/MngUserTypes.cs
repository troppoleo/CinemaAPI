using AutoMapper;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CinemaBL.Mapper
{
    public class MngUserTypes : Profile
    {
        //public List<CinemaDTO.UserTypeDTO> GetUserTypes()
        //{
        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<CinemaDAL.Models.UserType, CinemaDTO.UserTypeDTO>());
        //    var map = config.CreateMapper();

        //    CinemaDAL.Models.UserType utDAL;
        //    using (var ctx = new CinemaDAL.Models.CinemaContext())
        //    {
        //        utDAL = ctx.UserTypes.FirstOrDefault();
        //    }

        //    CinemaDTO.UserTypeDTO ut = map.Map<CinemaDTO.UserTypeDTO>(utDAL);
        //    return ut;

        //    //AutoMapper.Mapper

        //    //List<CinemaDTO.UserTypes> ll = new List<CinemaDTO.UserTypes>();

        //    //using (var ctx = new CinemaDAL.Models.CinemaContext())
        //    //{
        //    //    ctx.UserTypes
        //    //}
        //}



        public MngUserTypes()
        {
            //https://dev.to/moe23/add-automapper-to-net-6-3fdn
            // per fare anche il contrario:
            //CreateMap<CinemaDAL.Models.UserType, UserTypeDTO>().ReverseMap();
            CreateMap<CinemaDAL.Models.UserType, UserTypeDTO>();

        }
    }
}