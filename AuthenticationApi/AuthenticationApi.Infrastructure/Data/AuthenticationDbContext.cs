using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Domain.Entities;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
            : base(options) { }

        public DbSet<AppUser> Users { get; set; } = null!;
    }
}
