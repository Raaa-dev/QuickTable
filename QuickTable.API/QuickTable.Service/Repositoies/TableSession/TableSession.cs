using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Exceptions;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.TableSession.Dto;
using QRCoder;
using System.Drawing;
using System.IO;

namespace QuickTable.Service.Repositoies.TableSession
{
    public class TableSession (QuickTableContext _context, IMapper _mapper) : ITableSession
    {
        private const int SESSION_EXPIRY = 1;
        public async Task<TableResolveDto> ResolveTableByQrAsync(string token)
        {
            // 1. Find the QR and include the Table
            var qr = await _context.TableQrCodes
                .Include(q => q.Table)
                .FirstOrDefaultAsync(q => q.QrToken == token && q.IsActive == true)
                ?? throw new CustomException("Invalid QR Code");

            int tableId = qr.TableId;

            // 2. Get or create a session (automatically closes expired sessions)
            var session = await GetOrCreateSessionAsync(tableId);

            return new TableResolveDto
            {
                TableId = qr.TableId,
                Table = qr.Table.TableNumber,
                SessionId = session.Id // optionally include the session id for orders
            };
        }

        public async Task GenerateQrAsync(int tableId)
        {
            var token = Guid.NewGuid().ToString("N");

            var qr = new TableQrCode
            {
                TableId = tableId,
                QrToken = token,
                IsActive = true
            };

            _context.TableQrCodes.Add(qr);
            await _context.SaveChangesAsync();
        }

        // Get existing session or create new one if expired
        public async Task<Models.TableSession> GetOrCreateSessionAsync(int tableId)
        {
            // Find active session that is not expired
            var session = await _context.TableSessions
                .FirstOrDefaultAsync(s =>
                    s.TableId == tableId &&
                    s.Status == "Active" &&
                    s.StartedAt >= DateTime.Now.AddHours(-SESSION_EXPIRY)
                );

            if (session == null)
            {
                // Create new session
                session = new Models.TableSession
                {
                    TableId = tableId,
                    Status = "Active",
                    StartedAt = DateTime.Now
                };
                _context.TableSessions.Add(session);
                await _context.SaveChangesAsync();
            }

            return session;
        }

        public async Task CloseSessionAsync(int sessionId)
        {
            var session = await _context.TableSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId)
                ?? throw new CustomException("Session not found");

            session.Status = "Closed";
            session.EndAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }
        public async Task AutoCloseExpiredSessionsAsync()
        {
            var expiredSessions = await _context.TableSessions
                .Where(s => s.Status == "Active" && s.StartedAt < DateTime.Now.AddHours(-SESSION_EXPIRY))
                .ToListAsync();

            foreach (var s in expiredSessions)
            {
                s.Status = "Closed";
                s.EndAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public byte[] GenerateQrCode(string token)
        {
            string url = $"https://localhost:7295/api/v1/Table/resolve?token={token}";

            using (var qrGenerator = new QRCodeGenerator())
            using (var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrData))
            using (var bitmap = qrCode.GetGraphic(20))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
