
using System.Security.Claims;

namespace LouiseTieDyeStore.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<int>> CheckInUser(string userName)
        {
            var response = new ServiceResponse<int>();

            User user = null;

            if (await UserExists(userName))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userName));
            }
            else
            {
                user = new User
                {
                    Email = userName
                };

                await Register(user);
            }

            response.Data = user.Id;

            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower()
                .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private async Task Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
