using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.Table;
using QuickTable.Service.Repositoies.Table.Dto;
using QuickTable.Service.Repositoies.TableSession;

namespace QuickTable.API.Controller.v1
{
    public class TableController(ITableRepository _tableRepository, ITableSession _tableSession) : BaseController
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

        [HttpPost("Create")]

        public async Task<IActionResult> CreateAsync([FromBody] TableWriteDto dtoCreate)
        {
            var result = await _tableRepository.CreateAsync(dtoCreate);
            return Ok(result);
        }

        [HttpPut("Update/{id}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TableUpdateDto dtoUpdate)
        {
            var result = await _tableRepository.UpdateAsync(id, dtoUpdate);
            return Ok(result);
        }


        [HttpGet("resolve")]
        public async Task<IActionResult> Resolve(string token)
        {
            var result = await _tableSession.ResolveTableByQrAsync(token);
            return Ok(result);
        }

        [HttpPost("generate-qr/{tableId}")]
        public async Task<IActionResult> GenerateQr(int tableId)
        {
            await _tableSession.GenerateQrAsync(tableId);
            return Ok("QR Generated");
        }

        [HttpGet("table/{token}/qr")]
        public IActionResult GenerateQrCode(string token)
        {
            var qrBytes = _tableSession.GenerateQrCode(token);
            return File(qrBytes, "image/png");
        }
    }
}
