namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(Order order);
        Task<ServiceResponse<string>> GetLastOrderIdByUserEmail(string email);
        Task<ServiceResponse<Order>> GetOrder(Guid orderId);
        Task<ServiceResponse<Order>> GetAdminOrder(Guid orderId);
        Task<ServiceResponse<string>> ChangeOrderStatus(Guid orderId, string status);
        Task<ServiceResponse<OrderPageResults>> GetOrders(int page, string? statusFilter = null, bool orderByNewest = true);
        Task<ServiceResponse<OrderPageResults>> GetAdminOrders(int page, string? statusFilter = null, bool orderByNewest = true);
        Task<ServiceResponse<OrderPageResults>> SearchOrders(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetOrderSearchSuggestions(string searchText);
    }
}
