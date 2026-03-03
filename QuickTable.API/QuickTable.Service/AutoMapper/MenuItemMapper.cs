using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuickTable.Service.Repositoies.MenuCategory.Dto;
using QuickTable.Service.Repositoies.MenuItem.Dto;

namespace QuickTable.Service.AutoMapper
{
    public class MenuItemMapper : Profile
    {
        public MenuItemMapper()
        {
            AllowNullDestinationValues = null;
            CreateMap<Models.MenuItem, MenuItemReadDto>().ReverseMap();
            CreateMap<Models.MenuItem, MenuItemWriteDto>().ReverseMap();
            CreateMap<Models.MenuItem, MenuItemUpdateDto>().ReverseMap();
        }
    }
}
