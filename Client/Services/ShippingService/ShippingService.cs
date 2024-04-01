
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

        public async Task<string> GetShippingCost(Address address)
        {
            ShippingInfoDTO shippingInfo = new ShippingInfoDTO
            {
                LineOne = address.LineOne,
                LineTwo = address.LineTwo,
                City = address.City,
                Zip = int.Parse(address.Zip)
            };

            var result = await _publicClient.PostAsJsonAsync("api/shipping/validate-address", shippingInfo);
            var response = (await result.Content.ReadFromJsonAsync<ServiceResponse<string>>()).Data;

            return response;
        }
    }
}
