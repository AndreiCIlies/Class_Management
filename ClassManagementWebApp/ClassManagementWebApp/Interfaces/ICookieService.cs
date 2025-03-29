namespace ClassManagementWebApp.Interfaces;

public interface ICookieService
{
    Task<string> GetCookie(string name);
    Task SetCookie(string name, string value, int days);
    Task DeleteCookie(string name);
}