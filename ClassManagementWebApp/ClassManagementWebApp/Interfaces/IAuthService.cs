using ClassManagementWebApp.DTO;

namespace ClassManagementWebApp.Interfaces;

public interface IAuthService
{
    Task<bool> Login(AuthModel model);
    Task<bool> Register(AuthModel model);
    Task LogOut();
}