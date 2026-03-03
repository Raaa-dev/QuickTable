using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuickTable.Service.Repositoies.MenuCategory.Dto;

namespace QuickTable.Service.AutoMapper
{
    public class MenuCategoryMapper : Profile
    {
        public MenuCategoryMapper()
        {
            AllowNullDestinationValues = null;
            CreateMap<Models.MenuCategory, MenuCategoryReadDto>().ReverseMap();
            CreateMap<Models.MenuCategory, MenuCategoryWriteDto>().ReverseMap();
            CreateMap<Models.MenuCategory, MenuCategoryUpdateDto>().ReverseMap();
        }
    }
}
