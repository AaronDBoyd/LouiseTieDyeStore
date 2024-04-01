namespace LouiseTieDyeStore.Client.Services.ShippingService
{
    public interface IShippingService
    {
        Task<string> GetShippingCost(Address address);
    }
}
