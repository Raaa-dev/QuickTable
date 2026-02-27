using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuickTable.Service.Repositoies.User.Dto;

namespace QuickTable.Service.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            AllowNullDestinationValues = null;
            CreateMap<Models.User, UserReadDto>().ReverseMap();
            CreateMap<Models.User, UserUpdateDto>().ReverseMap();
            CreateMap<Models.User, UserWriteDto>().ReverseMap();
        }
    }
}
