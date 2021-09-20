using AutoMapper;
using ContactManager.Model;
using ContactManager.Model.DTO;
using System;

namespace ContactManager.Common
{
    public class AutoMapperInit: Profile
    {
        public AutoMapperInit()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, ResponseDTO>().ReverseMap();
            CreateMap<User, RegisterRequestDTO>().ReverseMap();
        }
    }
}
