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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            base.OnModelCreating(modelBuilder);

            /// Set Default Model ForeignKey is Restrict
            modelBuilder.SetRelationship(DeleteBehavior.Restrict);

            /// Set IsConcurrencyToken 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var rowVersionProperty = entityType.GetProperties().FirstOrDefault(e => e.Name == "RowVersion");
                if (rowVersionProperty != null && rowVersionProperty.ClrType == typeof(byte[])) modelBuilder.Entity(entityType.ClrType).Property("RowVersion").IsRowVersion().IsConcurrencyToken();
            }

            //modelBuilder.ExcludeMigration<BloodType>();
            //modelBuilder.SetToView<vwProduct>("E1AW_vwProduct");

        }
    }
}
