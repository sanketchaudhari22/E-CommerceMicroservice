using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interface;
using OrderApi.Domain.Entities;
using Polly;

namespace OrderApi.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderInterface _orderInterface;
        private readonly ResiliencePipeline _resiliencePipeline;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IHttpClientFactory httpClientFactory,
                            IOrderInterface orderInterface,
                            ResiliencePipeline resiliencePipeline)
        {
            _httpClientFactory = httpClientFactory;
            _orderInterface = orderInterface;
            _resiliencePipeline = resiliencePipeline;
        }

        public async Task<ProductDto?> GetProduct(int productId)
        {
            var client = _httpClientFactory.CreateClient("ProductApi");
            var response = await client.GetAsync($"/api/products/{productId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }

        public async Task<AppUserDto?> GetUser(int userId)
        {
            var client = _httpClientFactory.CreateClient("AuthApi");
            var response = await client.GetAsync($"/api/Authentication/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<AppUserDto>();
        }

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

        public async Task<IEnumerable<string>> GetOrdersByClientId(int clientId)
        {
            var orders = await _orderInterface.GetOrdersByAsync(o => o.ClientId == clientId);
            return orders.Select(o => $"Order {o.Id} - {o.OrderedDate:d}");
        }
    }
}
