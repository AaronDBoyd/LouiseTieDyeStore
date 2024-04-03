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
    }
}
