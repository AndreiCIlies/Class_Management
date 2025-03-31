using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace ClassManagementWebApp.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<IdentityUser>();
        return passwordHasher.HashPassword(null, password);
    }
}