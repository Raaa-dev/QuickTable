using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.User;
using QuickTable.Service.Repositoies.User.Dto;

namespace QuickTable.API.Controller.v1
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(QuickTableContext context, IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] UserFilterDto filter)
        {
            var result = await _userRepository.GetAllAsync(search, filter);
            return Ok(result);
        }
    }
}
