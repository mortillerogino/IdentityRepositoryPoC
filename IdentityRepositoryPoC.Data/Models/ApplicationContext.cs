using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace IdentityRepositoryPoC.Data.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, 
        IdentityRole, string, 
        ApplicationUserClaim, 
        IdentityUserRole<string>, 
        IdentityUserLogin<string>, 
        IdentityRoleClaim<string>, 
        IdentityUserToken<string>>
    {
        public ApplicationContext(DbContextOptions dbContext) 
            : base(dbContext)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            builder.Entity<ApplicationUserClaim>().ToTable("ApplicationUserClaim");
        }
    }
}
