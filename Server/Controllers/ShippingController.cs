using LouiseTieDyeStore.Shared.FedExRequestResponse.AuthToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LouiseTieDyeStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        // Is Valid Address
        [HttpPost("validate-address")] // Calling this endpoint will Validate address AND Return Shipping Rate Quote
        public async Task<ActionResult<ServiceResponse<ShippingResponse>>> ValidateAddress(ShippingInfoDTO shippingInfo)
        {
            var result = await _shippingService.ValidateShippingAddress(shippingInfo);
            return Ok(result);
        }

        // Get Shipping Rate Quotes without Validating Address
        //[HttpPost("rate-quote")]
        //public async Task<ActionResult<ServiceResponse<ShippingResponse>>> GetShippingRateQuote(ShippingInfoDTO shippingInfo)
        //{
        //    var result = await _shippingService.GetShippingRateQuote(shippingInfo);
        //    return Ok(result);
        //}

        // Get Auth Token
        //[HttpPost("authToken")] 
        //public async Task<ActionResult<ServiceResponse<string>>> GetAuthToken()
        //{
        //    var token = await _shippingService.GetAuthToken();

        //    var response = new ServiceResponse<string>
        //    {
        //        Data = token
        //    };

        //    return Ok(response);
        //}
    }
}
