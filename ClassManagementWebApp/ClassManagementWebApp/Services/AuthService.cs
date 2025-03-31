using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClassManagementWebApp.Services;

public class AuthService(IAccessTokenService _accessTokenServices, NavigationManager _navigationManager, IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

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

    public async Task LogOut()
    {
        var token = await _accessTokenServices.GetToken();
        await _accessTokenServices.RemoveToken();
        _navigationManager.NavigateTo("/login");
    }
}