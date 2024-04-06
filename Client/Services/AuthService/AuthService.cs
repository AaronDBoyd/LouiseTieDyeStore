using Blazored.LocalStorage;
using Blazored.SessionStorage;
using LouiseTieDyeStore.Client.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LouiseTieDyeStore.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _privateClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ISessionStorageService _sessionStorage;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient http, 
            AuthenticationStateProvider authStateProvider, 
            ISessionStorageService sessionStorage,
            ILocalStorageService localStorage,
            IConfiguration configuration)
        {
            _privateClient = http;
            _authStateProvider = authStateProvider;
            _sessionStorage = sessionStorage;
            _localStorage = localStorage;
            _configuration = configuration;
        }

        public async Task CheckInUser()
        {
            string userName = await GetAuthenticatedUsername();
            var result = await _privateClient.PostAsJsonAsync("api/auth/check-in", userName);

            var userId = (await result.Content.ReadFromJsonAsync<ServiceResponse<int>>()).Data;
        }

        public async Task<string> GetAuthenticatedUsername()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            string userName = authState.User.Identity.Name;
            return userName;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task StoreSessionIdToken()
        {
            var authority = _configuration["Auth0:Authority"];
            var clientId = _configuration["Auth0:ClientId"];

            var sessionToken = await _sessionStorage.GetItemAsync<OidcUserSessionToken>($"oidc.user:{authority}:{clientId}");

            var idToken = sessionToken.id_token;

            await _localStorage.SetItemAsync("id_token", idToken);
        }

        public async Task<bool> IsUserAnAdmin()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var roleClaims = authState.User.FindAll(ClaimTypes.Role).ToList();
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }

            return roles.Contains("Admin");
        }
    }
}
