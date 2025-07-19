using UserManagementService.Api.Models;

namespace UserManagementService.Api.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        Task<int> CompleteAsync();
    }
}