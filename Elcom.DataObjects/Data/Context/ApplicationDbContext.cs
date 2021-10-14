using Elcom.DataObjects.Models.MasterData;
using Elcom.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Elcom.DataObjects
{
    public class ApplicationDbContext : DbContext
    {
        IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var appDbConfiguratopnSection = _configuration.GetSection("ConnectionStrings");
            var connectionStrings = appDbConfiguratopnSection.Get<ConnectionStrings>();
            optionsBuilder.UseSqlServer(connectionStrings.AppConnectionString).EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException("ModelBuilder is NULL");
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AccountSubscription>().HasIndex(t => new { t.CompanyName }).IsUnique(true);

           #region User Managment
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique(true);
            //modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique(true);
            modelBuilder.Entity<Privilege>().HasIndex(p => p.Action).IsUnique(true);
            #endregion

           #region Master data
           
            #endregion
       modelBuilder.Seed();

        }
        #region Common
        public DbSet<MasterDataTransactionalHistory> MasterDataTransactionalHistory { get; set; }
        #endregion
        #region  subscription and client token
        //public DbSet<AccountSubscription> AccountSubscription { get; set; }
        public DbSet<PasswordRecovery> PasswordRecovery { get; set; }
        #endregion
        #region User Managment
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Privilege> Privilege { get; set; }
        public DbSet<RolePrivilege> RolePrivilege { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<Menus> Menus { get; set; }
        #endregion
        #region Client User managment
        public DbSet<ClientUser> ClientUser { get; set; }
        public DbSet<ClientRole> ClientRole { get; set; }
        public DbSet<ClientPrivilege> ClientPrivilege { get; set; }
        public DbSet<ClientRolePrivilege> ClientRolePrivilege { get; set; }
        public DbSet<ClientUserToken> ClientUserToken { get; set; }
        #endregion
        #region Master Data
        //public DbSet<Supplier> Suppliers { get; set; }

        #endregion
        #region Operational

        //

        //

        #endregion
    }
}