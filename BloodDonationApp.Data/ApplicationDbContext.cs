using BloodDonationApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodDonationApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //public DbSet<BloodGroup> BloodGroups { get; set; }
        //public DbSet<UserDetail> UserDetails { get; set; }
        //public DbSet<UserType> UserTypes { get; set; }
        //public DbSet<UserTypeRole> UserTypeRoles { get; set; }
        //public DbSet<Navigation> Navigations { get; set; }
        //public DbSet<RoleNavigation> RoleNavigations { get; set; }
        //public DbSet<BloodBank> BloodBanks { get; set; }
        //public DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<UserToken>().ToTable("UserToken");
        }
    }
}
