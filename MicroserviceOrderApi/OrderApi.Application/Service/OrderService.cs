using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interface;
using OrderApi.Domain.Entities;
using Polly; // for resilience

namespace OrderApi.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IOrderInterface _orderInterface;
        private readonly ResiliencePipeline _resiliencePipeline;

        public OrderService(HttpClient httpClient, IOrderInterface orderInterface, ResiliencePipeline resiliencePipeline)
        {
            _httpClient = httpClient;
            _orderInterface = orderInterface;
            _resiliencePipeline = resiliencePipeline;
        }

        // Get Product
        public async Task<ProductDto?> GetProduct(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }

        // Get User
        public async Task<AppUserDto?> GetUser(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/users/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AppUserDto>();
        }

        // Get Order Details By Order ID
        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var order = await _orderInterface.FindByIdAsync(orderId)
                        ?? throw new Exception($"Order with ID {orderId} not found.");

            var productDto = await _resiliencePipeline.ExecuteAsync(async token =>
                await GetProduct(order.ProductId))
                ?? throw new Exception($"Product with ID {order.ProductId} not found.");

            var appUserDto = await _resiliencePipeline.ExecuteAsync(async token =>
                await GetUser(order.ClientId))
                ?? throw new Exception($"User with ID {order.ClientId} not found.");

            return new OrderDetailsDto(
                order.Id,
                productDto.Id,
                appUserDto.Id,
                appUserDto.Email,
                productDto.Name,
                order.PurchaseQuantity,
                productDto.Price,
                productDto.Price * order.PurchaseQuantity,
                order.OrderedDate
            );
        }


        // Extra method from IOrderService
        public async Task<IEnumerable<string>> GetOrdersByClientId(int clientId)
        {
            var orders = await _orderInterface.GetOrdersByAsync(o => o.ClientId == clientId);
            return orders.Select(o => $"Order {o.Id} - {o.OrderedDate:d}");
        }

        
    }
}
