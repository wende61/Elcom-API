using EProcurement.DataObjects.Models.MasterData;
using EProcurement.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using EProcurement.DataObjects.Models.Operational;
using CargoProrationAPI.DataObjects.Models.Operational;

namespace EProcurement.DataObjects
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

            modelBuilder.Entity<AccountSubscription>().HasIndex(t => new { t.CompanyName }).IsUnique(true);

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
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<BusinessCategoryType> BusinessCategoryTypes { get; set; }
        public DbSet<BusinessCategory> BusinessCategories { get; set; }
        public DbSet<VendorType> VendorTypes{ get; set; }
        public DbSet<PurchaseGroup> purchaseGroups{ get; set; }
        public DbSet<RequirmentPeriod> RequirmentPeriods{ get; set; }
        public DbSet<SupplierBusinessCategory> SupplierBusinessCategory { get; set; }
        #endregion
        #region Operational
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<PRDelegateTeam> PRDelegateTeams { get; set; }
        public DbSet<PRApprover> PRApprovers { get; set; }
        public DbSet<HotelAccommodation> HotelAccommodationRequests { get; set; }
        public DbSet<HARApprover> HARApprovers { get; set; }
        public DbSet<HARDelegateTeam> HARDelegateTeams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<RequestForDocument> RequestForDocumentations { get; set; }
        public DbSet<RequestForDocAttachment> RequestForDocAttachments { get; set; }
        public DbSet<RequestForDocumentApproval> RequestForDocumentApproval { get; set; }
        //
        public DbSet<Criterion> Criteria { get; set; }
        public DbSet<CriteriaGroup> CriteriaGroup { get; set; }
        public DbSet<TechnicalEvaluation> TechnicalEvaluation { get; set; }
        //
        public DbSet<FinancialEvaluation> FinancialEvaluations { get; set; }
        public DbSet<FinancialCriteriaGroup> FinancialCriteriaGroups { get; set; }
        public DbSet<FinancialCriteria> FinancialCriterias { get; set; }
        public DbSet<FinancialCriteriaItem> FinancialCriteriaItems { get; set; }
        public DbSet<HotelAccommodationCriteria> HotelAccommodationCriteria { get; set; }
        public DbSet<TenderInvitation> TenderInvitations { get; set; }
        public DbSet<SupplierTenderInvitation> SupplierTenderInvitations { get; set; }
        public DbSet<ShortListApproval> ShortListApprovals { get; set; }
        public DbSet<ShortListedSupplier> ShortListedSuppliers { get; set; }
        public DbSet<ShortListApprover> ShortListApprovers { get; set; }
        public DbSet<TenderInvitationFloat> TenderInvitationFloats { get; set; }
        public DbSet<SuppliersProposalAttachment> SuppliersProposalAttachments { get; set; }
        public DbSet<SuppliersTenderProposal> SuppliersTenderProposals { get; set; }
        public DbSet<StoredFiles> StoredFiles { get; set; }
        #endregion
    }
}