using MediatR;
using UserManagementService.Api.Repositories;

namespace UserManagementService.Api.CQRS.Commands;

public class DeleteUserCommand : IRequest<int>
{
    public int UserId { get; }
    public DeleteUserCommand(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");
        }
        UserId = userId;
    }
}
public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, int>
{
    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");
        }
        unitOfWork.Users.Delete(user); // Changed from Remove to Delete
        return await unitOfWork.CompleteAsync();
    }
}

