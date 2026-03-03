using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.MenuItem.Dto;
using QuickTable.Service.Repositoies.Order.Dto;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.AutoMapper
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            AllowNullDestinationValues = null;
            CreateMap<Models.Order, OrderReadDto>()
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            // OrderItem mapping
            CreateMap<OrderItem, OrderItemReadDto>();

            // MenuItem mapping
            //CreateMap<MenuItem, MenuItemReadDto>();

            CreateMap<Models.Order, OrderWriteDto>().ReverseMap();
            CreateMap<Models.Order, OrderUpdateDto>().ReverseMap();
        }
    }
}
