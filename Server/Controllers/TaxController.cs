using LouiseTieDyeStore.Client.Pages.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LouiseTieDyeStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly ISalesTaxService _taxService;

        public TaxController(ISalesTaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<TaxRate>>>> GetTaxRates()
        {
            var result = await _taxService.GetRates();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<TaxRate>>>> UpdateTaxRates(List<TaxRate> taxRates)
        {
            var result = await _taxService.UpdateRates(taxRates);

            return Ok(result);
        }
    }
}
