
using Blazored.LocalStorage;
using static System.Net.WebRequestMethods;

namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _publicClient;
        private readonly HttpClient _privateClient;
        private readonly IAuthService _authService;
        private readonly ILocalStorageService _localStorage;

        public OrderService(PublicClient publicClient, 
            HttpClient privateClient,
            IAuthService authService, 
            ILocalStorageService localStorage)
        {
            _publicClient = publicClient.Client;
            _privateClient = privateClient;
            _authService = authService;
            _localStorage = localStorage;
        }

        public async Task<string> GetLastOrderIdByUserEmail()
        {
            string email = string.Empty;

            if (await _authService.IsUserAuthenticated())
            {
                email = await _authService.GetAuthenticatedUsername();
            }
            else if (await _localStorage.ContainKeyAsync("guestCheckoutEmail"))
            {
                email = await _localStorage.GetItemAsync<string>("guestCheckoutEmail");
            }

            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<string>>($"api/order/lastOrder/{email}");

            return result.Data;
        }

        public async Task<ServiceResponse<Order>> GetOrder(Guid orderId)
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<Order>>($"api/order/{orderId}");

            return result;
        }

        public async Task<string> PlaceOrder(CheckoutDTO checkout)
        {
            var result = await _publicClient.PostAsJsonAsync("api/payment/checkout", checkout);
            var url = await result.Content.ReadAsStringAsync();
            return url;
        }

        public async Task ChangeOrderStatus(OrderStatusRequest request)
        {
            _ = await _privateClient.PutAsJsonAsync("api/order/status", request);
        }
    }
}
