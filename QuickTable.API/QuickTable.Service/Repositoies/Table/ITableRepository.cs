using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.Table
{
    public interface ITableRepository
    {
        Task<PagedResponse<TableReadDto>> GetAllAsync(string? search, TableFilterDto filter);
        Task<TableReadDto> GetByIdAsync(int id);
        Task<TableReadDto> CreateAsync(TableWriteDto createDto);
        Task<TableReadDto> UpdateAsync(int id, TableUpdateDto updateDto);
    }
}
