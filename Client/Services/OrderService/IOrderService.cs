namespace LouiseTieDyeStore.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder(CheckoutDTO checkout);
        Task<string> GetLastOrderIdByUserEmail();
    }
}
