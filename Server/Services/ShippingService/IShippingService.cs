using LouiseTieDyeStore.Shared.FedExRequestResponse.AuthToken;
using LouiseTieDyeStore.Shared.FedExRequestResponse.ValidateAddress;

namespace LouiseTieDyeStore.Server.Services.ShippingService
{
    public interface IShippingService
    {
        Task<string> GetAuthToken();
        Task<ServiceResponse<ShippingResponse>> GetShippingRateQuote(ShippingResponse shippingResponse, ShippingInfoDTO shippingInfo, string? authToken = null);
        Task<ServiceResponse<ShippingResponse>> ValidateShippingAddress(ShippingInfoDTO shippingInfo);
    }
}
