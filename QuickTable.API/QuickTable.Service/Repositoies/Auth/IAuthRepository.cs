using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Repositoies.Auth.Dto;

namespace QuickTable.Service.Repositoies.Auth
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> RefreshAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
    }

}
