using AutoMapper;
using ContactManager.Model;
using ContactManager.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.API
{
    public class MapperInit : Profile
    {
        public MapperInit()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
