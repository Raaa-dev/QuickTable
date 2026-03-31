using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuItemWriteDto dtoCreate)
        {
            var result = await _menuItemRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] MenuItemUpdateDto dtoUpdate)
        {
            var result = await _menuItemRepository.UpdateAsync(id, dtoUpdate);
            return Ok(result);
        }

        // PUT api/menu-items/5/image
        [Authorize]
        [HttpPut("{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var result = await _menuItemRepository.UploadImageAsync(id, file);
            return Ok(result);
        }

        // DELETE api/menu-items/5/image
        [Authorize]
        [HttpDelete("{id}/image")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            await _menuItemRepository.DeleteImageAsync(id);
            return Ok(new { message = "Image deleted successfully!" });
        }
    }
}

