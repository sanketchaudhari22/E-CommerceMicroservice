using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversion;
using OrderApi.Application.Interface;
using OrderApi.Application.Service;
using OrderApi.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderInterface _orderInterface;

        public OrderController(IOrderInterface orderInterface)
        {
            _orderInterface = orderInterface;
        }

        // GET: api/order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderInterface.GetAllAsync();
            if (!orders.Any()) return NotFound("No orders found.");

            var list = orders.ToDtoList(); // Converts to List<OrderDto>
            return Ok(list);
        }

        // GET: api/order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _orderInterface.FindByIdAsync(id);
            if (order == null) return NotFound($"Order with ID {id} not found.");

            var dto = order.ToDto(); // Converts single Order → OrderDto
            return Ok(dto);
        }

        // GET: api/order/client/5
        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByClient(int clientId)
        {
            var orders = await _orderInterface.GetOrdersByAsync(o => o.ClientId == clientId);
            if (!orders.Any()) return NotFound($"No orders found for client ID {clientId}.");

            var list = orders.ToDtoList();
            return Ok(list);
        }

        // GET: api/order/details/5
        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int orderId, [FromServices] IOrderService orderService)
        {
            try
            {
                var details = await orderService.GetOrderDetails(orderId);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        // POST: api/order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            if (orderDto == null) return BadRequest("Order data is null.");

            var entity = orderDto.ToEntity();
            var createdOrder = await _orderInterface.CreateAsync(entity);

            var dto = createdOrder.ToDto();
            return CreatedAtAction(nameof(GetOrderById), new { id = dto.Id }, dto);
        }

        // PUT: api/order/5
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, OrderDto orderDto)
        {
            var existingOrder = await _orderInterface.FindByIdAsync(id);
            if (existingOrder == null) return NotFound($"Order with ID {id} not found.");

            var updatedEntity = orderDto.ToEntity();
            updatedEntity.Id = id;

            var updatedOrder = await _orderInterface.UpdateAsync(updatedEntity);

            var dto = updatedOrder.ToDto();
            return Ok(dto);
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _orderInterface.FindByIdAsync(id);
            if (order == null) return NotFound($"Order with ID {id} not found.");

            var result = await _orderInterface.DeleteAsync(order);
            return result ? NoContent() : StatusCode(500, "Error occurred while deleting the order.");
        }
    }
}
