using OrderApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Service
{
    internal interface IOrderService
    {
        Task<IEnumerable<string>> GetOrdersByClientId(int clientId);


        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
