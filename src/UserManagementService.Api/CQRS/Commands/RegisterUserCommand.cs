using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;
using UserManagementService.Api.Services;

namespace UserManagementService.Api.CQRS.Commands;
public class RegisterUserCommand : IRequest<AuthResult>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
}

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService) : IRequestHandler<RegisterUserCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.Users.GetByEmailAsync(request.Email) != null)
            throw new Exception("Email already registered");

        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = SecurityExtension.HashPassword(request.Password, salt),
            Salt = salt,
            PhoneNumber = request.PhoneNumber,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.CompleteAsync();

        return new AuthResult
        {
            Email = user.Email,
            AccessToken = tokenService.GenerateJwtToken(user),
            RefreshToken = await tokenService.GenerateAndStoreRefreshTokenAsync(user)
        };
    }
}
