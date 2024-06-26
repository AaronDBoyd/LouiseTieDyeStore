﻿
using LouiseTieDyeStore.Shared;
using System.Security.Claims;

namespace LouiseTieDyeStore.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<int> GetUserId()
        {
            string userEmail = _httpContextAccessor.HttpContext.User
                                       .FindFirstValue(ClaimTypes.Name);

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
            return user.Id;
        }

        public async Task<string> GetUserEmail()
        {
            return _httpContextAccessor.HttpContext.User
                                       .FindFirstValue(ClaimTypes.Name);
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

        public async Task Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
