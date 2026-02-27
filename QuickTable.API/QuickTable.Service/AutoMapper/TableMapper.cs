using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.AutoMapper
{
    public class TableMapper : Profile
    {
        public TableMapper()
        {
            AllowNullDestinationValues = null;
            CreateMap<Models.Table, TableReadDto>().ReverseMap();
            CreateMap<Models.Table, TableWriteDto>().ReverseMap();
            CreateMap<Models.Table, TableUpdateDto>().ReverseMap();

        }
    }
}
