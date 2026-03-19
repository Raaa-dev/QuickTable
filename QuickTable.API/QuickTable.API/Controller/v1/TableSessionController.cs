using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.Table;
using QuickTable.Service.Repositoies.TableSession;

namespace QuickTable.API.Controller.v1
{
    public class TableSessionController(ITableSession _tableSession, QuickTableContext _context) : BaseController    
    {
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

        [HttpGet("Table/{token}/QR")]
        public IActionResult GenerateQrCode(string token)
        {
            var qrBytes = _tableSession.GenerateQrCode(token);
            return File(qrBytes, "image/png");
        }

        // GET /api/v1/Table/session/{sessionId}
        [HttpGet("Session/{sessionId}")]
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
        [HttpPost("ResetTableBySession/{sessionId}")]
        public async Task<IActionResult> CloseSession(int sessionId)
        {
            await _tableSession.CloseSessionAsync(sessionId); // already exists ✅
            return Ok(new { message = "Session closed" });
        }

        // POST /api/v1/Table/close-by-table/{tableId}  ← close all active sessions for a table
        [HttpPost("ResetTable/{tableId}")]
        public async Task<IActionResult> CloseSessionByTable(int tableId)
        {
            await _tableSession.CloseSessionByTableAsync(tableId);
            return Ok(new { message = "Table sessions closed" });
        }

        // In your TableController
        [HttpGet("with-session-status")]
        public async Task<IActionResult> GetWithSessionStatus()
        {
            var tables = await _context.Tables
                .Select(t => new {
                    t.Id,
                    t.TableNumber,
                    HasActiveSession = _context.TableSessions
                        .Any(s => s.TableId == t.Id && s.Status == "Active")
                })
                .OrderBy(t => t.TableNumber)
                .ToListAsync();

            return Ok(new { data = tables });
        }
    }
}
