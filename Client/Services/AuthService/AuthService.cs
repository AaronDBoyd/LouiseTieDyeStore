using Microsoft.AspNetCore.Components.Authorization;

namespace LouiseTieDyeStore.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _privateClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _privateClient = http;
            _authStateProvider = authStateProvider;
        }

        public async Task CheckInUser()
        {         
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            string userName = authState.User.Identity.Name;
            var result = await _privateClient.PostAsJsonAsync("api/auth/check-in", userName);

            var userId = (await result.Content.ReadFromJsonAsync<ServiceResponse<int>>()).Data;
        }

        public Task<bool> IsUserAuthenticated()
        {
            throw new NotImplementedException();
        }
    }
}
