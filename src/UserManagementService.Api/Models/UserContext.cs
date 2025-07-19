using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Api.Models
{
    public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}