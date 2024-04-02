
using System.Net.Http;

namespace LouiseTieDyeStore.Client.Services.ShippingService
{
    public class ShippingService : IShippingService
    {
        private readonly HttpClient _publicClient;

        public ShippingService(PublicClient publicClient)
        {
            _publicClient = publicClient.Client;
        }
        
        public async Task<string> GetShippingCost(ShippingInfoDTO shippingInfo)
        {

            var result = await _publicClient.PostAsJsonAsync("api/shipping/rate-quote", shippingInfo);
            var response = (await result.Content.ReadFromJsonAsync<ServiceResponse<string>>()).Data;

            return response;
        }
    }
}
