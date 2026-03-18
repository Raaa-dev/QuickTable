using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QuickTable.Service.Repositoies.Order.Dto;

namespace QuickTable.Service.Shared
{

    // ITelegramService.cs
    public interface ITelegramNotificationService
    {
        Task SendOrderNotificationAsync(OrderReadDto order, string tableNumber = null);
    }

    // TelegramService.cs
    public class TelegramNotificationService : ITelegramNotificationService
    {
        private readonly string _botToken;
        private readonly string _chatId;
        private readonly HttpClient _httpClient;

        public TelegramNotificationService(IConfiguration config, HttpClient httpClient)
        {
            _botToken = config["Telegram:BotToken"];
            _chatId = config["Telegram:ChatId"];
            _httpClient = httpClient;
        }

        public async Task SendOrderNotificationAsync(OrderReadDto order, string tableNumber = null)
        {
            var message = BuildMessage(order, tableNumber);
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var payload = new
            {
                chat_id = _chatId,
                text = message,
                parse_mode = "HTML"
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(url, content);
        }

        private string BuildMessage(OrderReadDto order, string tableNumber)
        {
            var sb = new StringBuilder();
            sb.AppendLine("🍽️ <b>NEW ORDER RECEIVED</b>");
            sb.AppendLine($"📋 Order: <b>{order.OrderNumber}</b>");

            if (!string.IsNullOrEmpty(tableNumber))
                sb.AppendLine($"🪑 Table: <b>{tableNumber}</b>");

            sb.AppendLine($"🔖 Session ID: {order.TableSessionId}");
            sb.AppendLine($"📌 Status: {order.Status}");
            sb.AppendLine();
            sb.AppendLine("🛒 <b>Items:</b>");

            foreach (var item in order.Items)
            {
                sb.AppendLine($"  • {item.MenuItem?.Name} x{item.Quantity} — ${item.Subtotal:F2}");
            }

            sb.AppendLine();
            sb.AppendLine($"💵 <b>Total: ${order.TotalAmount:F2}</b>");
            sb.AppendLine($"🕐 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            return sb.ToString();
        }
    }
}
