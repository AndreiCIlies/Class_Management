using ClassManagementWebApp.Interfaces;
using Microsoft.JSInterop;

namespace ClassManagementWebApp.Services;

public class CookieService(IJSRuntime _jSRuntime) : ICookieService
{
    public async Task<string> GetCookie(string name)
    {
        return await _jSRuntime.InvokeAsync<string>("getCookie", name);
    }

    public async Task SetCookie(string name, string value, int days)
    {
        await _jSRuntime.InvokeVoidAsync("setCookie", name, value, days);
    }

    public async Task DeleteCookie(string name)
    {
        await _jSRuntime.InvokeVoidAsync("deleteCookie", name);
    }
}