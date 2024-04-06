namespace LouiseTieDyeStore.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task CheckInUser();
        Task<bool> IsUserAuthenticated();
        Task<bool> IsUserAnAdmin();
        Task StoreSessionIdToken();
        Task<string> GetAuthenticatedUsername();
    }
}
