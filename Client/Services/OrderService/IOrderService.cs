namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder(CheckoutDTO checkout);
        Task<string> GetLastOrderIdByUserEmail();
        Task<ServiceResponse<Order>> GetOrder(Guid orderId);
        Task ChangeOrderStatus(OrderStatusRequest request);
    }
}
