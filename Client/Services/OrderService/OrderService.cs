
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
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

        public List<OrderOverviewResponse> Orders { get; set; } = new List<OrderOverviewResponse>();
        public string? StatusFilter { get; set; }
        public string Message { get; set; } = "Loading Orders...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action OrdersChanged;


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
            ServiceResponse<Order> result;

            if (await _authService.IsUserAnAdmin())
            {
                result = await _privateClient.GetFromJsonAsync<ServiceResponse<Order>>($"api/order/admin/{orderId}");
            }
            else
            {
                result = await _privateClient.GetFromJsonAsync<ServiceResponse<Order>>($"api/order/{orderId}");
            }
    
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

        public async Task GetOrders(OrderPageRequest request)
        {
            var result = await _privateClient.PostAsJsonAsync("api/order", request);

            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderPageResults>>();

            if (result != null && response.Data != null && response.Data.Orders != null)
            {
                Orders = response.Data.Orders;
                CurrentPage = response.Data.CurrentPage;
                PageCount = response.Data.Pages;
            }

            if (!response.Success)
            {
                Message = "No Orders Found";
                Orders.Clear();
            }

            OrdersChanged.Invoke();
        }

        public async Task GetAdminOrders(OrderPageRequest request)
        {
            var result = await _privateClient.PostAsJsonAsync("api/order/admin", request);

            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<OrderPageResults>>();

            if (result != null && response.Data != null && response.Data.Orders != null)
            {
                Orders = response.Data.Orders;
                CurrentPage = response.Data.CurrentPage;
                PageCount = response.Data.Pages;
            }

            if (!response.Success)
            {
                Message = "No Orders Found";
                Orders.Clear();
            }

            OrdersChanged.Invoke();
        }

        public async Task<List<string>> GetOrderSearchSuggestions(string searchText)
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/order/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchOrders(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<OrderPageResults>>($"api/order/search/{searchText}/{page}");

            if (result != null && result.Data != null && result.Data.Orders != null)
            {
                Orders =    result.Data.Orders;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (!result.Success)
            {
                Message = "No Orders Found";
                Orders.Clear();
            }

            OrdersChanged.Invoke();
        }
    }
}
