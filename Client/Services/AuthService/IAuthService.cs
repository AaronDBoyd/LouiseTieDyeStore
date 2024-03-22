namespace LouiseTieDyeStore.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task CheckInUser();
        Task<bool> IsUserAuthenticated();
        Task StoreSessionIdToken();
    }
}
