using OrderApi.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Application.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<string>> GetOrdersByClientId(int clientId);
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
