using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Helpers;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.User.Dto;

namespace QuickTable.Service.Repositoies.User
{
    public class UserRepository(QuickTableContext _context, IMapper _mapper) : IUserRepository
    {
        public async Task<PagedResponse<UserReadDto>> GetAllAsync(string? search, UserFilterDto filter)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    var val = search.ToLower();
                    query = query.Where(u => (u.UserName ?? "").ToLower().Contains(val));
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
                return new PagedResponse<UserReadDto>
                {
                    Data = _mapper.Map<List<UserReadDto>>(results),
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
    }
}
