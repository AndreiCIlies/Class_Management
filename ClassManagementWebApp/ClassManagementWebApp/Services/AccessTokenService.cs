using ClassManagementWebApp.Interfaces;

namespace ClassManagementWebApp.Services;

public class AccessTokenService(ICookieService _cookieService) : IAccessTokenService
{
    public async Task SetToken(string token)
    {
        await _cookieService.SetCookie("access_token", token, 1);
    }

    public async Task<string> GetToken()
    {
        return await _cookieService.GetCookie("access_token");
    }

    public async Task RemoveToken()
    {
        await _cookieService.DeleteCookie("access_token");
    }
}
