
using static System.Net.WebRequestMethods;

namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _publicClient;

        public OrderService(PublicClient publicClient)
        {
            _publicClient = publicClient.Client;
        }

        public async Task<string> PlaceOrder(CheckoutDTO checkout)
        {
            var result = await _publicClient.PostAsJsonAsync("api/payment/checkout", checkout);
            var url = await result.Content.ReadAsStringAsync();
            return url;
        }
    }
}
