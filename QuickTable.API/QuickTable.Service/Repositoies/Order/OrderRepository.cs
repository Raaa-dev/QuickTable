using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickTable.Service.Enum;
using QuickTable.Service.Exceptions;
using QuickTable.Service.Helpers;
using QuickTable.Service.Models;
using QuickTable.Service.Repositoies.MenuItem.Dto;
using QuickTable.Service.Repositoies.Order.Dto;
using QuickTable.Service.Shared;

namespace QuickTable.Service.Repositoies.Order
{
    public class OrderRepository(QuickTableContext _context, IMapper _mapper, ITelegramNotificationService _telegram) : IOrderRepository
    {
        public async Task<PagedResponse<OrderReadDto>> GetAllAsync(string? search, OrderFilterDto filter)
        {
            try
            {
                var query = _context.Orders.AsQueryable();

                if (filter.TableSessionId != 0)
                {
                    query = query.Where(u => u.TableSessionId == filter.TableSessionId);
                }

                if (filter.Status != null)
                {
                    query = query.Where(u => u.Status == filter.Status);
                }

                query = query.Include(u => u.TableSession)  
                            .Include(u => u.OrderItems)       
                            .ThenInclude(oi => oi.MenuItem);
                var totalRecords = await query.CountAsync();
                var results = await query
                    .OrderByDescending(u => u.Id)
                    .Skip((filter.PageNo - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                // Map entities
                var mappedResults = _mapper.Map<List<OrderReadDto>>(results);

                return new PagedResponse<OrderReadDto>
                {
                    Data = mappedResults,
                    TotalRecords = totalRecords,
                    PageNo = filter.PageNo,
                    PageSize = filter.PageSize
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderReadDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Orders
                    .Include(o => o.TableSession)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefaultAsync(o => o.Id == id) ?? throw new CustomException($"Cannot find Order with Id {id}!");
                return _mapper.Map<OrderReadDto>(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<OrderReadDto> CreateAsync(int sessionId, List<OrderItemWriteDto> itemsDto)
        {
            // 1. Get session
            var session = await _context.TableSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Status == "Active")
                ?? throw new CustomException("Session not found or expired");

            // 2. Create Order
            var order = new Models.Order
            {
                TableSessionId = session.Id,
                Status = "Pending",
                TotalAmount = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 3. Generate OrderNumber
            order.OrderNumber = $"ORD-{order.Id:D7}";

            // 4. Add OrderItems
            foreach (var dto in itemsDto)
            {
                var menuItem = await _context.MenuItems.FindAsync(dto.MenuItemId);
                if (menuItem == null) throw new CustomException("MenuItem not found");

                var item = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = menuItem.Id,
                    Quantity = dto.Quantity,
                    Price = menuItem.Price,
                    Subtotal = dto.Quantity * menuItem.Price
                };
                order.OrderItems.Add(item);
            }

            // 5. Calculate total
            order.TotalAmount = order.OrderItems.Sum(i => i.Subtotal ?? 0);

            await _context.SaveChangesAsync();

            var result =  _mapper.Map<OrderReadDto>(order);

            // 7. Send Telegram notification (fire-and-forget, won't block response)
            _ = Task.Run(() => _telegram.SendOrderNotificationAsync(result, session.Table?.TableNumber?.ToString()));

            return result;
        }

        //public async Task<OrderReadDto> CreateAsync(int tableId, List<OrderItemWriteDto> itemsDto)
        //{
        //    // 1. Get or create TableSession
        //    var session = await _context.TableSessions
        //        .FirstOrDefaultAsync(s => s.TableId == tableId && s.Status == "Active");

        //    if (session == null)
        //    {
        //        session = new Models.TableSession
        //        {
        //            TableId = tableId,
        //            Status = "Active",
        //            StartedAt = DateTime.Now
        //        };
        //        _context.TableSessions.Add(session);
        //        await _context.SaveChangesAsync();
        //    }

        //    // 2. Create Order
        //    var order = new Models.Order
        //    {
        //        TableSessionId = session.Id,
        //        Status = "Pending",
        //        TotalAmount = 0 // will calculate later
        //    };

        //    // 3. Save first to get Order Id for OrderNumber
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    // 4. Generate OrderNumber based on Id
        //    order.OrderNumber = $"ORD-{order.Id:D7}";

        //    // 5. Add OrderItems
        //    foreach (var dto in itemsDto)
        //    {
        //        var menuItem = await _context.MenuItems.FindAsync(dto.MenuItemId);
        //        if (menuItem == null) throw new CustomException("MenuItem not found");

        //        var item = new OrderItem
        //        {
        //            OrderId = order.Id,
        //            MenuItemId = menuItem.Id,
        //            Quantity = dto.Quantity,
        //            Price = menuItem.Price,
        //            Subtotal = dto.Quantity * menuItem.Price
        //        };
        //        order.OrderItems.Add(item);
        //    }

        //    // 6. Calculate total
        //    order.TotalAmount = order.OrderItems.Sum(i => i.Subtotal ?? 0);

        //    await _context.SaveChangesAsync();

        //    return _mapper.Map<OrderReadDto>(order);
        //}

        public async Task<OrderReadDto> UpdateAsync(int id, OrderUpdateDto dtoUpdate)
        {
            var entity = await _context.Orders.FindAsync(id) ?? throw new CustomException($"Cannot find Order with Id {id}!");
            _mapper.Map(dtoUpdate, entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrderReadDto>(entity);
        }
    }
}
