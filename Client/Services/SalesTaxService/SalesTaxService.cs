
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Client.Services.SalesTaxService
{
    public class SalesTaxService : ISalesTaxService
    {
        private readonly HttpClient _httpClient;

        public SalesTaxService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaxRate>> GetTaxRates()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<TaxRate>>>("api/tax");

            return result.Data;
        }

        public async Task<List<TaxRate>> UpdateRates(List<TaxRate> taxRates)
        {
            var result = await _httpClient.PutAsJsonAsync("api/tax", taxRates); 

            var rateList = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<TaxRate>>>()).Data;

            return rateList;
        }
    }
}
