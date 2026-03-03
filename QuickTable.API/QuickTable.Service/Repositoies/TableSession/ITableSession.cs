using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Repositoies.TableSession.Dto;

namespace QuickTable.Service.Repositoies.TableSession
{
    public interface ITableSession
    {
        Task<TableResolveDto> ResolveTableByQrAsync(string token);
        Task GenerateQrAsync(int tableId);
        Task <Models.TableSession> GetOrCreateSessionAsync(int tableId);
        byte[] GenerateQrCode(string token);
    }
}
