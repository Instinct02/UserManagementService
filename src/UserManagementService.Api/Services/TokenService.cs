using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;
namespace UserManagementService.Api.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    Task<string> GenerateAndStoreRefreshTokenAsync(User user);
}

public class TokenService(IConfiguration config, IUnitOfWork unitOfWork) : ITokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim("uid", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
            new Claim("phoneNumber", user.PhoneNumber),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }

    public async Task<string> GenerateAndStoreRefreshTokenAsync(User user)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // or 30, depending on your policy

        unitOfWork.Users.Update(user);
        await unitOfWork.CompleteAsync();

        return refreshToken;
    }
}
