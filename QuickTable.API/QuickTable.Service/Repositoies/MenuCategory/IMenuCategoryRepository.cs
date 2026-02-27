using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;
using QuickTable.Service.Repositoies.MenuCategory.Dto;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.MenuCategory
{
    public interface IMenuCategoryRepository
    {
        Task<PagedResponse<MenuCategoryReadDto>> GetAllAsync(string? search, MenuCategoryFilterDto filter);
        Task<MenuCategoryReadDto> GetByIdAsync(int id);
        Task<MenuCategoryReadDto> CreateAsync(MenuCategoryWriteDto dtoCreate);
        Task<MenuCategoryReadDto> UpdateAsync(int id,  MenuCategoryUpdateDto dtoUpdate);
    }
}
