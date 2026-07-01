using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityEFCoreWebAPI.Models;

namespace IdentityEFCoreWebAPI.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable(name: "User");
            });
            builder.Entity<ApplicationRole>(entity => {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable(name: "UserRole");
            });
            builder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable(name: "UserClaim");
            });
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable(name: "UserLogin");
            });
            builder.Entity<IdentityRoleClaim<Guid>>(entity => {
                entity.ToTable(name: "RoleClaim");
            });
            builder.Entity<IdentityUserToken<Guid>>(entity => {
                entity.ToTable(name: "UserToken");
            });
        }

    }
}
