using System.Security.Cryptography;
using System.Text;

namespace UserManagementService.Api.Services;

public static class SecurityExtension
{
    public static string HashPassword(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        return Convert.ToBase64String(SHA256.HashData(combined));
    }
}
