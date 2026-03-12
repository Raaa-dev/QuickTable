using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.MenuCategory;
using QuickTable.Service.Repositoies.MenuCategory.Dto;

namespace QuickTable.API.Controller.v1
{
    public class MenuCategoryController(IMenuCategoryRepository _menuCategoryRepository) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] MenuCategoryFilterDto filter)
        {
            var result = await _menuCategoryRepository.GetAllAsync(search, filter);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _menuCategoryRepository.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] MenuCategoryWriteDto dtoCreate)
        {
            var result = await _menuCategoryRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }
        //[HttpPut("Update/{id}")]
        //public async Task<IActionResult> UpdateAsync(int id, [FromBody] MenuCategoryUpdateDto dtoUpdate)
        //{
        //    var result = await _menuCategoryRepository.UpdateAsync(id, dtoUpdate);
        //    return Ok(result);
        //}
    }
}
