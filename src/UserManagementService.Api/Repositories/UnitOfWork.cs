using UserManagementService.Api.Models;

namespace UserManagementService.Api.Repositories;

public class UnitOfWork(UserContext context) : IUnitOfWork
{
    private IGenericRepository<User>? _users;

    public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(context);

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
}
