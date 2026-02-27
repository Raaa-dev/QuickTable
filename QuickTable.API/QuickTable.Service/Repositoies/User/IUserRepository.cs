using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;
using QuickTable.Service.Repositoies.User.Dto;

namespace QuickTable.Service.Repositoies.User
{
    public interface IUserRepository
    {
        Task<PagedResponse<UserReadDto>> GetAllAsync (string? search, UserFilterDto filter);
    }
}
