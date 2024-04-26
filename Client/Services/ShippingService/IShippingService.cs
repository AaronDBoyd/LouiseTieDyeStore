namespace LouiseTieDyeStore.Client.Services.ShippingService
{
    public interface IShippingService
    {
        List<string> LocalZipCodes { get; }
        Task<string> GetShippingCost(ShippingInfoDTO shippingInfo);
    }
}
