using MediatR;
using UserManagementService.Api.Models;
using UserManagementService.Api.Repositories;

namespace UserManagementService.Api.CQRS.Queries
{
    public class GetUserByIdQuery(int id) : IRequest<User?>
    {
        public int Id { get; set; } = id;
    }
    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, User?>
    {
        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(request.Id));
            }
            return await unitOfWork.Users.GetByIdAsync(request.Id);
        }
    }
}
