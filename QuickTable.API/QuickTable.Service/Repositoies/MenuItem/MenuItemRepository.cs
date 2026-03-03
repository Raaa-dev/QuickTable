using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Exceptions;
using QuickTable.Service.Helpers;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.MenuCategory.Dto;
using QuickTable.Service.Repositoies.MenuItem.Dto;

namespace QuickTable.Service.Repositoies.MenuItem
{
    public class MenuItemRepository (QuickTableContext _context, IMapper _mapper) : IMenuItemRepository
    {
        public async Task<PagedResponse<MenuItemReadDto>> GetAllAsync(string? search, MenuItemFilterDto filter)
        {
            try
            {
                var query = _context.MenuItems.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(u => (u.Name ?? "").ToLower().Contains(val));
                }

                if (filter.CategoryId != 0)
                {
                    query = query.Where(u => u.CategoryId == filter.CategoryId);
                }

                if (filter.IsActive != null)
                {
                    query = query.Where(u => u.IsActive == filter.IsActive);
                }

                var totalRecords = await query.CountAsync();
                var results = await query
                    .OrderByDescending(u => u.Id)
                    .Skip((filter.PageNo - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return new PagedResponse<MenuItemReadDto>
                {
                    Data = _mapper.Map<List<MenuItemReadDto>>(results),
                    TotalRecords = totalRecords,
                    PageNo = filter.PageNo,
                    PageSize = filter.PageSize
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MenuItemReadDto> GetByIdAsync(int id)
        {
            var entiry = await _context.MenuItems.FindAsync(id) ?? throw new CustomException($"Cannot find Menu item with Id {id}!"); ;
            return _mapper.Map<MenuItemReadDto>(entiry);
        }

        public async Task<MenuItemReadDto> CreateAsync(MenuItemWriteDto dtoCreate)
        {

            if (string.IsNullOrEmpty(dtoCreate.Name))
            {
                throw new CustomException("Name is required!");
            }

            if (dtoCreate.CategoryId <= 0)
            {
                throw new CustomException("Category is required!");
            }

            if (dtoCreate.Price <= 0)
            {
                throw new CustomException("Price is required!");
            }
            var entity = _mapper.Map<Models.MenuItem>(dtoCreate);
            _context.MenuItems.Add(entity);
            await _context.SaveChangesAsync();
            return GetByIdAsync(entity.Id).Result;
        }

        public async Task<MenuItemReadDto> UpdateAsync(int id, MenuItemUpdateDto dtoUpdate)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoUpdate.Name))
                {
                    throw new CustomException("Name is required!");
                }

                if (dtoUpdate.CategoryId <= 0)
                {
                    throw new CustomException("Category is required!");
                }

                if (dtoUpdate.Price <= 0)
                {
                    throw new CustomException("Price is required!");
                }

                var entity = await _context.MenuItems.FindAsync(id) ?? throw new CustomException($"Cannot find Menu item with Id {id}!");
                _mapper.Map(dtoUpdate, entity);
                _context.MenuItems.Update(entity);
                await _context.SaveChangesAsync();
                return GetByIdAsync(entity.Id).Result;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
