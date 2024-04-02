namespace LouiseTieDyeStore.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> CheckInUser(string userName);
        Task<bool> UserExists(string email);
        Task<int> GetUserId();
        Task<string> GetUserEmail();
        Task<User> GetUserByEmail(string email);
        Task Register(User user);
    }
}
