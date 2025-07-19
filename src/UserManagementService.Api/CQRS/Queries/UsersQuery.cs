using MediatR;
using UserManagementService.Api.Models;
using System.Collections.Generic;
using UserManagementService.Api.Repositories;

namespace UserManagementService.Api.CQRS.Queries
{
    public class UsersQuery : IRequest<PaginatedResult<User>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }


    public class UsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UsersQuery, PaginatedResult<User>>
    {
        public async Task<PaginatedResult<User>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var users = await unitOfWork.Users.GetAllAsync();
            var totalCount = users.Count();
            var pagedUsers = users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            return new PaginatedResult<User>
            {
                Items = pagedUsers,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }


    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}