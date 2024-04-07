using System.Reflection.Metadata.Ecma335;

namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public interface IOrderService
    {
        event Action OrdersChanged;
        List<OrderOverviewResponse> Orders { get; set; }
        string? StatusFilter { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task<string> PlaceOrder(CheckoutDTO checkout);
        Task<string> GetLastOrderIdByUserEmail();
        Task<ServiceResponse<Order>> GetOrder(Guid orderId);
        Task ChangeOrderStatus(OrderStatusRequest request);
        Task GetOrders(OrderPageRequest request);
        Task GetAdminOrders(OrderPageRequest request);
        Task<List<string>> GetOrderSearchSuggestions(string searchText);
        Task SearchOrders(string searchText, int page);
    }
}
