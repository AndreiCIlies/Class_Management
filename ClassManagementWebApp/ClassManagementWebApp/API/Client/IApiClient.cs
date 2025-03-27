using ClassManagementWebApp.API.Models;

namespace ClassManagementWebApp.API.Client;

public interface IApiClient
{
    Task<bool> Register(RegisterModel model);
    Task<bool> Login(LoginModel model);
    Task Logout();
}