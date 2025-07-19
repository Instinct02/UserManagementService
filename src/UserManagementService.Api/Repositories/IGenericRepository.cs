using UserManagementService.Api.Models;

namespace UserManagementService.Api.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
