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


        [HttpGet("Resolve")]
        public async Task<IActionResult> Resolve(string token)
        {
            var result = await _tableSession.ResolveTableByQrAsync(token);
            return Ok(result);
        }

        [HttpPost("Generate-QR/{tableId}")]
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

        // GET /api/v1/Table/session/{sessionId}
        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetSession(int sessionId)
        {
            var session = await _tableSession.GetSessionByIdAsync(sessionId);
            if (session == null) return NotFound();

            return Ok(new
            {
                id = session.Id,
                status = session.Status,
                tableId = session.TableId,
                startedAt = session.StartedAt,
                endAt = session.EndAt
            });
        }

        // POST /api/v1/Table/close/{sessionId}  ← for admin panel
        [HttpPost("close/{sessionId}")]
        public async Task<IActionResult> CloseSession(int sessionId)
        {
            await _tableSession.CloseSessionAsync(sessionId); // already exists ✅
            return Ok(new { message = "Session closed" });
        }

        // POST /api/v1/Table/close-by-table/{tableId}  ← close all active sessions for a table
        [HttpPost("close-by-table/{tableId}")]
        public async Task<IActionResult> CloseSessionByTable(int tableId)
        {
            await _tableSession.CloseSessionByTableAsync(tableId);
            return Ok(new { message = "Table sessions closed" });
        }
    }
}
