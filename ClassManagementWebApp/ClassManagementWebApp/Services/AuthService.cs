using ClassManagementWebApp.DTO;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace ClassManagementWebApp.Services
{
    public class AuthService
    {
        private readonly AccessTokenService _accessTokenServices;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClient;

        public AuthService(AccessTokenService accessTokenServices, NavigationManager navigationManager, IHttpClientFactory httpClientFactory)
        {
            _accessTokenServices = accessTokenServices;
            _navigationManager = navigationManager;
            _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");
        }

        public async Task<bool> Login(AuthModel model)
        {
            var status = await _httpClient.PostAsJsonAsync("auth/login", model);
            if(status.IsSuccessStatusCode)
            {
                var token = await status.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthResponse>(token);
                await _accessTokenServices.SetToken(result.token);
                return true;
            }
            return false;
        }

        public async Task<bool> Register(AuthModel model)
        {
            var status = await _httpClient.PostAsJsonAsync("auth/register", model);
            return status.IsSuccessStatusCode;
        }
    }
}