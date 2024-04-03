
using Blazored.LocalStorage;
using static System.Net.WebRequestMethods;

namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _publicClient;
        private readonly IAuthService _authService;
        private readonly ILocalStorageService _localStorage;

        public OrderService(PublicClient publicClient, 
            IAuthService authService, 
            ILocalStorageService localStorage)
        {
            _publicClient = publicClient.Client;
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

            var result = await _publicClient.GetFromJsonAsync<ServiceResponse<string>>($"api/Order/lastOrder/{email}");

            return result.Data;
        }

        public async Task<string> PlaceOrder(CheckoutDTO checkout)
        {
            var result = await _publicClient.PostAsJsonAsync("api/payment/checkout", checkout);
            var url = await result.Content.ReadAsStringAsync();
            return url;
        }
    }
}
