namespace LouiseTieDyeStore.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(string userEmail);
    }
}
