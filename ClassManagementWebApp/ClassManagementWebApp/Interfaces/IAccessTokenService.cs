namespace ClassManagementWebApp.Interfaces;

public interface IAccessTokenService
{
    Task SetToken(string token);
    Task<string> GetToken();
    Task RemoveToken();
}