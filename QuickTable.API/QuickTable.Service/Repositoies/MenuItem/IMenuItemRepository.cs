using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QuickTable.Service.Helpers;
using QuickTable.Service.Repositoies.MenuCategory.Dto;
using QuickTable.Service.Repositoies.MenuItem.Dto;

namespace QuickTable.Service.Repositoies.MenuItem
{
    public interface IMenuItemRepository
    {
        Task<PagedResponse<MenuItemReadDto>> GetAllAsync(string? search, MenuItemFilterDto filter);
        Task<MenuItemReadDto> GetByIdAsync(int id);
        Task<MenuItemReadDto> CreateAsync(MenuItemWriteDto dtoCreate);
        Task<MenuItemReadDto> UpdateAsync(int id, MenuItemUpdateDto dtoUpdate);
        Task<MenuItemReadDto> UploadImageAsync(int id, IFormFile file); 
        Task DeleteImageAsync(int id);
    }
}
