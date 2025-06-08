using AW.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Entities;

namespace UMS.Core.Contexts
{
    public class UMSDbContext : DbContext
    {
        public static bool ValidateDatabaseModel = false;
        public UMSDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            if (ValidateDatabaseModel)
            {
                var pendingMigrations = this.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Any() || !this.Database.GetMigrations().Contains(this.Database.GetAppliedMigrations().ToList().Last()))
                {
                    throw new Exception("The Database Schema Changed.Please Check the migration version");

                }
            }
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppToken> AppTokens { get; set; }
        public DbSet<AppUserAccess> AppUserAccesses { get; set; }
        public DbSet<AppUserAccessGroup> AppUserAccessGroups { get; set; }
        public DbSet<AppUserAccessGroupModule> AppUserAccessGroupModules { get; set; }
        public DbSet<AppUserAccessGroupModuleAccess> AccessGroupModuleAccesses { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.InitContext();

            /// Set Default Model ForeignKey is Restrict
            modelBuilder.SetRelationship(DeleteBehavior.Restrict);

            //modelBuilder.ExcludeMigration<BloodType>();
            //modelBuilder.SetToView<vwProduct>("E1AW_vwProduct");

        }
    }
}
