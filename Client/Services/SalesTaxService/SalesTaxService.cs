
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Client.Services.SalesTaxService
{
    public class SalesTaxService : ISalesTaxService
    {
        private readonly HttpClient _publicClient;

        public SalesTaxService(PublicClient publicClient)
        {
            _publicClient = publicClient.Client;
        }

        public async Task<decimal> CalculateSalesTax(decimal subtotal, string state)
        {
            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<decimal>>($"api/tax/{state}");

            var rate = result.Data/100;

            var salesTax = Math.Round(subtotal * rate, 2);

            return salesTax;
        }

        public async Task<List<TaxRate>> GetTaxRates()
        {
            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<List<TaxRate>>>("api/tax");

            return result.Data;
        }

        public async Task<List<TaxRate>> UpdateRates(List<TaxRate> taxRates)
        {
            var result = await _publicClient.PutAsJsonAsync("api/tax", taxRates); 

            var rateList = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<TaxRate>>>()).Data;

            return rateList;
        }
    }
}
