using OrderApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OrderApi.Application.DTOs.Conversion
{
    public static class OrderConversion
    {
        // Single Order → OrderDto
        public static OrderDto ToDto(this Order order)
        {
            if (order == null) return null!;

            return new OrderDto(
                order.Id,
                order.ProductId,
                order.ClientId,
                order.PurchaseQuantity,
                order.OrderedDate
            );
        }

        // IEnumerable<Order> → List<OrderDto>
        public static List<OrderDto> ToDtoList(this IEnumerable<Order> orders)
        {
            if (orders == null || !orders.Any()) return new List<OrderDto>();
            return orders.Select(o => o.ToDto()).ToList();
        }

        // OrderDto → Order
        public static Order ToEntity(this OrderDto dto)
        {
            if (dto == null) return null!;

            return new Order
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                ClientId = dto.ClientId,
                PurchaseQuantity = dto.PurchaseQuantity,
                OrderedDate = dto.OrderedDate
            };
        }
    }
}
