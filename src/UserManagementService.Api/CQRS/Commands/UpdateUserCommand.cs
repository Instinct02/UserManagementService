using MediatR;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;

namespace UserManagementService.Api.CQRS.Commands;

public class UpdateUserCommand (User user) : IRequest<int>
{
    public User User { get; } = user ?? throw new ArgumentNullException(nameof(user), "User cannot be null");
}
public class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, int>
{
    public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.User == null)
        {
            throw new ArgumentNullException(nameof(request.User), "User cannot be null");
        }
        var existingUser = await unitOfWork.Users.GetByIdAsync(request.User.Id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {request.User.Id} not found");
        }

        // Update other properties as needed
        unitOfWork.Users.Update(existingUser);
        return await unitOfWork.CompleteAsync();
    }
}

