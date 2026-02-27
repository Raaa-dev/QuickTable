using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.Table;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.API.Controller.v1
{
    public class TableController(ITableRepository _tableRepository) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] TableFilterDto filter)
        {
            var result = await _tableRepository.GetAllAsync(search, filter);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _tableRepository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromBody] TableWriteDto dtoCreate)
        {
            var result = await _tableRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }

        [HttpPut("UpdateTable/{id}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TableUpdateDto dtoUpdate)
        {
            var result = await _tableRepository.UpdateAsync(id, dtoUpdate);
            return Ok(result);
        }
    }
}
