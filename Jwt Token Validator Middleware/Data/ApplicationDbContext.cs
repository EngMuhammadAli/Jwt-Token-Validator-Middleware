using Jwt_Token_Validator_Middleware.Models;
using Microsoft.EntityFrameworkCore;

namespace Jwt_Token_Validator_Middleware.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}
