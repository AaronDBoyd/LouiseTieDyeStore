namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(Order order);
        Task<ServiceResponse<string>> GetLastOrderIdByUserEmail(string email);
    }
}
