using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Helpers;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.Table
{
    public class TableRepository : ITableRepository
    {
        private readonly QuickTableContext _context;
        private readonly IMapper _mapper;

        public TableRepository(QuickTableContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
            var entiry = await _context.Tables.FindAsync(id);
            if (entiry == null)
            {
                throw new Exception("Table not found");
            }
            return _mapper.Map<TableReadDto>(entiry);
        }

        public async Task<TableReadDto> CreateAsync(TableWriteDto dtoCreate)
        {
            var entity = _mapper.Map<Models.Table>(dtoCreate);
            _context.Tables.Add(entity);
            await _context.SaveChangesAsync();
            return GetByIdAsync(entity.Id).Result;
        }

    }
}
