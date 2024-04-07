using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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

        [HttpGet("admin/{orderId}")]
        public async Task<ActionResult<ServiceResponse<Order>>> GetAdminOrder(Guid orderId)
        {
            var result = await _orderService.GetAdminOrder(orderId);

            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<OrderPageResults>>> GetAdminOrders(OrderPageRequest request)
        {
            var result = await _orderService.GetAdminOrders(request.Page, request.StatusFilter, request.OrderByNewest);

            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ServiceResponse<OrderPageResults>>> GetOrders(OrderPageRequest request)
        {
            var result = await _orderService.GetOrders(request.Page, request.StatusFilter, request.OrderByNewest);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("status")]
        public async Task<ActionResult<ServiceResponse<string>>> ChangeOrderStatus(OrderStatusRequest request)
        {
            var result = await _orderService.ChangeOrderStatus(request.OrderId, request.Status);
            
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetOrderSearchSuggestions(string searchText)
        {
            var result = await _orderService.GetOrderSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<OrderPageResults>>> SearchOrders(string searchText, int page = 1)
        {
            var result = await _orderService.SearchOrders(searchText, page);
            return Ok(result);
        }
    }
}
