using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;
using QuickTable.Service.Repositoies.Order.Dto;

namespace QuickTable.Service.Repositoies.Order
{
    public interface IOrderRepository
    {
        Task<PagedResponse<OrderReadDto>> GetAllAsync (string? search, OrderFilterDto filter);

        Task<OrderReadDto> GetByIdAsync(int id);
        Task<OrderReadDto> CreateAsync(int sessionId, List<OrderItemWriteDto> itemsDto);
        Task<OrderReadDto> UpdateAsync(int id, OrderUpdateDto dtoUpdate);
    }
}
