namespace LouiseTieDyeStore.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> CheckInUser(string userName);
        Task<bool> UserExists(string email);
        Task<int> GetUserId();
        Task<string> GetUserEmail();
    }
}
