using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.User;
using QuickTable.Service.Repositoies.User.Dto;

namespace QuickTable.API.Controller.v1
{
    public class UserController (IUserRepository _userRepository) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] UserFilterDto filter)
        {
            var result = await _userRepository.GetAllAsync(search, filter);
            return Ok(result);
        }
    }
}
