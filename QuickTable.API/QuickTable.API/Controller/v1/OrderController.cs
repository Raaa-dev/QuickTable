using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickTable.Service.Repositoies.Order;
using QuickTable.Service.Repositoies.Order.Dto;
using QuickTable.Service.Repositoies.Table.Dto;
using QuickTable.Service.Repositoies.TableSession;

namespace QuickTable.API.Controller.v1
{
    public class OrderController(IOrderRepository _orderRepository, ITableSession _tableSession) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search, [FromQuery] OrderFilterDto filter)
        {
            var result = await _orderRepository.GetAllAsync(search, filter);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _orderRepository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderRequestDto dto)
        {
            // 1. Resolve the table and get/create session
            var tableResolve = await _tableSession.ResolveTableByQrAsync(dto.QrToken);
            var session = await _tableSession.GetOrCreateSessionAsync(tableResolve.TableId);

            // 2. Create order for this session
            var order = await _orderRepository.CreateAsync(session.Id, dto.Items);

            return Ok(order);
        }

        [HttpPut("Update/{id}")]

        public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrderUpdateDto dtoUpdate)
        {
            var result = await _orderRepository.UpdateAsync(id, dtoUpdate);
            return Ok(result);
        }
    }
}
