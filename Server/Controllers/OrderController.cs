using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LouiseTieDyeStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("lastOrder/{email}")]
        public async Task<ActionResult<ServiceResponse<string>>> GetLastOrderIdByUserEmail(string email)
        {
            var result = await _orderService.GetLastOrderIdByUserEmail(email);

            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<Order>>> GetOrder(Guid orderId)
        {
            var result = await _orderService.GetOrder(orderId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("status")]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeOrderStatus(OrderStatusRequest request)
        {
            var result = await _orderService.ChangeOrderStatus(request.OrderId, request.Status);
            
            return Ok(result);
        }
    }
}
