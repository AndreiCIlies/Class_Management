using ClassManagementWebApp.API.Auth;
using ClassManagementWebApp.API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClassManagementWebApp.API.Client;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiAuthenticationStateProvider _authStateProvider;

    public ApiClient(HttpClient httpClient, ApiAuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _httpClient.BaseAddress = new Uri("https://localhost:7025/api/");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.AcceptLanguage.TryParseAdd(Thread.CurrentThread.CurrentUICulture.Name);
    }

    public async Task<bool> Register(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Login(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", model);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(responseBody);

            if (jsonDocument.RootElement.TryGetProperty("token", out JsonElement tokenElement))
            {
                string token = tokenElement.GetString();

                if (!string.IsNullOrEmpty(token))
                {
                    await _authStateProvider.MarkUserAsAuthenticated(token);
                    return true;
                }
            }
        }

        return false;
    }

    public async Task Logout()
    {
        await _authStateProvider.MarkUserAsLoggedOut();
    }
}