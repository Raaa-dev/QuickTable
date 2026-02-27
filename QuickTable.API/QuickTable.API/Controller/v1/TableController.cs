using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.Table;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.API.Controller.v1
{
    public class TableController : BaseController
    {
        private readonly ITableRepository _tableRepository;

        public TableController(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

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

        public async Task<IActionResult> CreateAsync(TableWriteDto dtoCreate)
        {
            var result = await _tableRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }
        
    }
}
