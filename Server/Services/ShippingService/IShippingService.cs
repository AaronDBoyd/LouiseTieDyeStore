using LouiseTieDyeStore.Shared.FedExRequestResponse.AuthToken;
using LouiseTieDyeStore.Shared.FedExRequestResponse.ValidateAddress;

namespace LouiseTieDyeStore.Server.Services.ShippingService
{
    public interface IShippingService
    {
        Task<string> GetAuthToken();
        Task<ServiceResponse<string>> GetShippingRateQuote(ShippingInfoDTO shippingInfo, string authToken);
        Task<ServiceResponse<string>> ValidateShippingAddress(ShippingInfoDTO shippingInfo);
    }
}
