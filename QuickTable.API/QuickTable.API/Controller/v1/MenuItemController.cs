using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.MenuItem;
using QuickTable.Service.Repositoies.MenuItem.Dto;

namespace QuickTable.API.Controller.v1
{
    public class MenuItemController(IMenuItemRepository _menuItemRepository) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] MenuItemFilterDto filter)
        {
            var result = await _menuItemRepository.GetAllAsync(search, filter);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _menuItemRepository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItemWriteDto dtoCreate)
        {
            var result = await _menuItemRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MenuItemUpdateDto dtoUpdate)
        {
            var result = await _menuItemRepository.UpdateAsync(id, dtoUpdate);
            return Ok(result);
        }
    }
}
