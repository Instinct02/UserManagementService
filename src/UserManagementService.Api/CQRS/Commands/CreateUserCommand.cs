using MediatR;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;

namespace UserManagementService.Api.CQRS.Commands;

public class CreateUserCommand(User user) : IRequest<int>
{
    public User User { get; } = user ?? throw new ArgumentNullException(nameof(user));
}

public class CreateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, int>
{
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.User == null)
        {
            throw new ArgumentNullException(nameof(request.User), "User cannot be null");
        }
        await unitOfWork.Users.AddAsync(request.User);
        return await unitOfWork.CompleteAsync();
    }
}