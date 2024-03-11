using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LouiseTieDyeStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("check-in")] 
        public async Task<ActionResult<ServiceResponse<int>>> CheckInUser([FromBody] string userName)
        {
            var response = await _authService.CheckInUser(userName);

            return Ok(response);
        }
    }
}
