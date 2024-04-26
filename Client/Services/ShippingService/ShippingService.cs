
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

        public List<string> LocalZipCodes => new List<string> { "98685", "98686", "98662", "98665", "98663", 
            "98661", "98664", "98684", "98683", "97203", "97217", "97211", "97218", "97220", "97213", "97212", 
            "97232", "97214", "97215", "97216", "97230", "97233", "97236", "97266", "97206", "97202"};

        public async Task<string> GetShippingCost(ShippingInfoDTO shippingInfo)
        {

            var result = await _publicClient.PostAsJsonAsync("api/shipping/rate-quote", shippingInfo);
            var response = (await result.Content.ReadFromJsonAsync<ServiceResponse<string>>()).Data;

            return response;
        }
    }
}
