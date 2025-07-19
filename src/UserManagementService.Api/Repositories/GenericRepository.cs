using Microsoft.EntityFrameworkCore;
using UserManagementService.Api.Models;

namespace UserManagementService.Api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly UserContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(UserContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<User?> GetByEmailAsync(string email) => await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);


        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}
