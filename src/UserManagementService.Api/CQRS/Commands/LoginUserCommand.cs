using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;
using UserManagementService.Api.Services;

namespace UserManagementService.Api.CQRS.Commands;
public class LoginUserCommand : IRequest<AuthResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService) : IRequestHandler<LoginUserCommand, AuthResult>
{
    public async Task<AuthResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(request.Email) ?? throw new Exception("Invalid credentials");
        var hash = SecurityExtension.HashPassword(request.Password, user.Salt);

        if (hash != user.Password)
            throw new Exception("Invalid credentials");

        return new AuthResult
        {
            Email = user.Email,
            AccessToken = tokenService.GenerateJwtToken(user),
            RefreshToken =  await tokenService.GenerateAndStoreRefreshTokenAsync(user)
        };
    }
}
