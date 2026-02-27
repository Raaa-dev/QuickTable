using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Exceptions;
using QuickTable.Service.Helpers;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.Table
{
    public class TableRepository(QuickTableContext _context, IMapper _mapper) : ITableRepository
    {
        public async Task<PagedResponse<TableReadDto>> GetAllAsync(string? search, TableFilterDto filter)
        {
            try
            {
                var query = _context.Tables.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(u => (u.TableNumber ?? "").ToLower().Contains(val));
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

                return new PagedResponse<TableReadDto>
                {
                    Data = _mapper.Map<List<TableReadDto>>(results),
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

        public async Task<TableReadDto> GetByIdAsync(int id)
        {
            var entiry = await _context.Tables.FindAsync(id) ?? throw new CustomException($"Cannot find Table with Id {id}!"); ;
            return _mapper.Map<TableReadDto>(entiry);
        }

        public async Task<TableReadDto> CreateAsync(TableWriteDto dtoCreate)
        {

            if (string.IsNullOrEmpty(dtoCreate.TableNumber)){
                throw new CustomException("TableNumber is required!");
            }
            if (dtoCreate.Capacity == 0 || dtoCreate == null)
            {
                throw new CustomException("Table Capacity is required!");
            }

            var tableNumberExists = await _context.Tables.AnyAsync(t => t.TableNumber == dtoCreate.TableNumber);
            if (tableNumberExists)
            {
                throw new ConflictException($"Table with TableNumber {dtoCreate.TableNumber} already exists!");
            }
            var entity = _mapper.Map<Models.Table>(dtoCreate);
            _context.Tables.Add(entity);
            await _context.SaveChangesAsync();
            return GetByIdAsync(entity.Id).Result;
        }

        public async Task<TableReadDto> UpdateAsync(int id ,TableUpdateDto dtoUpdate)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoUpdate.TableNumber))
                {
                    throw new CustomException("TableNumber is required!");
                }
                if (dtoUpdate.Capacity == 0 || dtoUpdate == null)
                {
                    throw new CustomException("Table Capacity is required!");
                }

                var tableNumberExists = await _context.Tables.AnyAsync(t => t.TableNumber == dtoUpdate.TableNumber);
                if (tableNumberExists)
                {
                    throw new CustomException($"Table with TableNumber {dtoUpdate.TableNumber} already exists!");
                }
                var entity = await _context.Tables.FindAsync(id) ?? throw new CustomException($"Cannot find Table with Id {id}!");
                _mapper.Map(dtoUpdate, entity);
                _context.Tables.Update(entity);
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
