using AutoMapper;
using CinemaDTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CinemaBL.Mapper
{
    public class MngUserTypes : Profile
    {       
        public MngUserTypes()
        {
            //https://dev.to/moe23/add-automapper-to-net-6-3fdn
            // per fare anche il contrario:
            //CreateMap<CinemaDAL.Models.UserType, UserTypeDTO>().ReverseMap();
            CreateMap<CinemaDAL.Models.UserType, UserTypeDTO>();

        }
    }
}