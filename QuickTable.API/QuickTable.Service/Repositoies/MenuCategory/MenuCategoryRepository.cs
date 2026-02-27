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
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.MenuCategory
{
    public class MenuCategoryRepository (QuickTableContext _context, IMapper _mapper) : IMenuCategoryRepository
    {
        public async Task<PagedResponse<MenuCategoryReadDto>> GetAllAsync(string? search, MenuCategoryFilterDto filter)
        {
            try
            {
                var query = _context.MenuCategories.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(u => (u.Name ?? "").ToLower().Contains(val));
                }

                if (filter.IsActive != null)
                {
                    query = query.Where(u => u.IsActive == filter.IsActive);
                }

                var totalRecords = await query.CountAsync();
                var results = await query
                    .Skip((filter.PageNo - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return new PagedResponse<MenuCategoryReadDto>
                {
                    Data = _mapper.Map<List<MenuCategoryReadDto>>(results),
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

        public async Task<MenuCategoryReadDto> GetByIdAsync(int id)
        {
            var entiry = await _context.MenuCategories.FindAsync(id) ?? throw new CustomException($"Cannot find Menu Category with Id {id}!"); ;
            return _mapper.Map<MenuCategoryReadDto>(entiry);
        }

        public async Task<MenuCategoryReadDto> CreateAsync(MenuCategoryWriteDto dtoCreate)
        {

            if (string.IsNullOrEmpty(dtoCreate.Name))
            {
                throw new CustomException("Name is required!");
            }
            var entity = _mapper.Map<Models.MenuCategory>(dtoCreate);
            _context.MenuCategories.Add(entity);
            await _context.SaveChangesAsync();
            return GetByIdAsync(entity.Id).Result;
        }

        public async Task<MenuCategoryReadDto> UpdateAsync(int id, MenuCategoryUpdateDto dtoUpdate)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoUpdate.Name))
                {
                    throw new CustomException("Name is required!");
                }
                var entity = await _context.MenuCategories.FindAsync(id) ?? throw new CustomException($"Cannot find Menu Category with Id {id}!");
                _mapper.Map(dtoUpdate, entity);
                _context.MenuCategories.Update(entity);
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
