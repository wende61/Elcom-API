using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSubscription",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    IsAccountActivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSubscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientPrivilege",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    Module = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPrivilege", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientRole",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_BusinessCategoryType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    CategoryType = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_BusinessCategoryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_CostCenter",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Station = table.Column<string>(nullable: false),
                    CostCenterName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_CostCenter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_Country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    ShortName = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_PurchaseGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Group = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_PurchaseGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_VendorType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_VendorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterDataTransactionalHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<long>(nullable: false),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    ActionType = table.Column<int>(nullable: false),
                    ActionTable = table.Column<int>(nullable: false),
                    ActionTakenBy = table.Column<string>(nullable: true),
                    RecordDataInJson = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataTransactionalHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Privilages = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AccountSubscriptionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_AccountSubscription_AccountSubscriptionId",
                        column: x => x.AccountSubscriptionId,
                        principalTable: "AccountSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRolePrivilege",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PrivilegeId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRolePrivilege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRolePrivilege_ClientPrivilege_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "ClientPrivilege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRolePrivilege_ClientRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ClientRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(maxLength: 30, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    VerificationToken = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsSuperAdmin = table.Column<bool>(nullable: false),
                    IsConfirmationEmailSent = table.Column<bool>(nullable: false),
                    IsAccountLocked = table.Column<bool>(nullable: false),
                    ClientRoleId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUser_ClientRole_ClientRoleId",
                        column: x => x.ClientRoleId,
                        principalTable: "ClientRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_BusinessCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Category = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BusinessCategoryTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_BusinessCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_BusinessCategory_MasterData_BusinessCategoryType_BusinessCategoryTypeId",
                        column: x => x.BusinessCategoryTypeId,
                        principalTable: "MasterData_BusinessCategoryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_Office",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    OfficeName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CostCenterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_Office", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_Office_MasterData_CostCenter_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "MasterData_CostCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_Person",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    ExtensionNumber = table.Column<string>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    CostCenterId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_Person_MasterData_CostCenter_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "MasterData_CostCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_Station",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    CityName = table.Column<string>(nullable: false),
                    CityCode = table.Column<string>(nullable: false),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_Station", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_Station_MasterData_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MasterData_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_RequirmentPeriod",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Period = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PurchaseGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_RequirmentPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_RequirmentPeriod_MasterData_PurchaseGroup_PurchaseGroupId",
                        column: x => x.PurchaseGroupId,
                        principalTable: "MasterData_PurchaseGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_Supplier",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    ContactEmail = table.Column<string>(nullable: false),
                    ContactPhoneNumber = table.Column<string>(nullable: true),
                    ContactTelNumber = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    SupplyCategoryDescription = table.Column<string>(nullable: true),
                    VendorTypeId = table.Column<long>(nullable: false),
                    StarType = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_Supplier_MasterData_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MasterData_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterData_Supplier_MasterData_VendorType_VendorTypeId",
                        column: x => x.VendorTypeId,
                        principalTable: "MasterData_VendorType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserToken",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    IssuedTime = table.Column<DateTime>(nullable: false),
                    ExpiryTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserToken_ClientUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ClientUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Privilege",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    Module = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsMorePermission = table.Column<bool>(nullable: false),
                    ClientUserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Privilege_ClientUser_ClientUserId",
                        column: x => x.ClientUserId,
                        principalTable: "ClientUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(maxLength: 30, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    VerificationToken = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsSuperAdmin = table.Column<bool>(nullable: false),
                    LoginAttemptCount = table.Column<int>(nullable: false),
                    LastLoginDateTime = table.Column<DateTime>(nullable: false),
                    IsConfirmationEmailSent = table.Column<bool>(nullable: false),
                    IsAccountLocked = table.Column<bool>(nullable: false),
                    AccountSubscriptionId = table.Column<long>(nullable: true),
                    RoleId = table.Column<long>(nullable: true),
                    PersonId = table.Column<long>(nullable: true),
                    accountType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AccountSubscription_AccountSubscriptionId",
                        column: x => x.AccountSubscriptionId,
                        principalTable: "AccountSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_HotelAccommodationRequest",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    RequestName = table.Column<string>(nullable: true),
                    HotelServiceType = table.Column<int>(nullable: false),
                    OriginatingSection = table.Column<int>(nullable: false),
                    CostCenterId = table.Column<long>(nullable: true),
                    StationId = table.Column<long>(nullable: true),
                    CountryId = table.Column<long>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ContractExpiredate = table.Column<DateTime>(nullable: true),
                    Commencementdate = table.Column<DateTime>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    CrewPattern = table.Column<string>(nullable: true),
                    AttachementPath = table.Column<string>(nullable: true),
                    RejectionRemark = table.Column<string>(nullable: true),
                    AssignRemark = table.Column<string>(nullable: true),
                    ReAssignRemark = table.Column<string>(nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    PRStatus = table.Column<int>(nullable: false),
                    RequestedBy = table.Column<long>(nullable: true),
                    RejectedBy = table.Column<long>(nullable: true),
                    AssignedTo = table.Column<long>(nullable: true),
                    AssignedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_HotelAccommodationRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Person_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Person_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_CostCenter_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "MasterData_CostCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "MasterData_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Person_RejectedBy",
                        column: x => x.RejectedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Person_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationRequest_MasterData_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "MasterData_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_ProcurementSection",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Section = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RequirmentPeriodId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_ProcurementSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_ProcurementSection_MasterData_RequirmentPeriod_RequirmentPeriodId",
                        column: x => x.RequirmentPeriodId,
                        principalTable: "MasterData_RequirmentPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterData_SupplierBusinessCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<long>(nullable: false),
                    BusinessCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterData_SupplierBusinessCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterData_SupplierBusinessCategory_MasterData_BusinessCategory_BusinessCategoryId",
                        column: x => x.BusinessCategoryId,
                        principalTable: "MasterData_BusinessCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterData_SupplierBusinessCategory_MasterData_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "MasterData_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivilege",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PrivilegeId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivilege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePrivilege_Privilege_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privilege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePrivilege_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecovery",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    VerificationToken = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    IsPasswordRecovered = table.Column<bool>(nullable: false),
                    RequestedOn = table.Column<DateTime>(nullable: false),
                    RecoveredOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordRecovery_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    IssuedTime = table.Column<DateTime>(nullable: false),
                    ExpiryTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_HotelARApprovers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PersonId = table.Column<long>(nullable: true),
                    HotelAccommodationId = table.Column<long>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_HotelARApprovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_HotelARApprovers_Operational_HotelAccommodationRequest_HotelAccommodationId",
                        column: x => x.HotelAccommodationId,
                        principalTable: "Operational_HotelAccommodationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelARApprovers_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_HotelARDelegateTeam",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PersonId = table.Column<long>(nullable: true),
                    HotelAccommodationId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_HotelARDelegateTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_HotelARDelegateTeam_Operational_HotelAccommodationRequest_HotelAccommodationId",
                        column: x => x.HotelAccommodationId,
                        principalTable: "Operational_HotelAccommodationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_HotelARDelegateTeam_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_PurchaseRequisition",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    RequestedGood = table.Column<string>(nullable: true),
                    ApprovedBudgetAmmount = table.Column<string>(nullable: true),
                    PurchaseType = table.Column<int>(nullable: false),
                    PurchaseGroupId = table.Column<long>(nullable: true),
                    RequirementPeriodId = table.Column<long>(nullable: true),
                    ProcurementSectionId = table.Column<long>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    CostCenterId = table.Column<long>(nullable: false),
                    Specification = table.Column<string>(nullable: true),
                    AttachementPath = table.Column<string>(nullable: true),
                    RejectionRemark = table.Column<string>(nullable: true),
                    AssignRemark = table.Column<string>(nullable: true),
                    ReAssignRemark = table.Column<string>(nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    PRStatus = table.Column<int>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    RequestedBy = table.Column<long>(nullable: true),
                    RejectedBy = table.Column<long>(nullable: true),
                    AssignedTo = table.Column<long>(nullable: true),
                    AssignedBy = table.Column<long>(nullable: true),
                    RequirmentPeriodId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_PurchaseRequisition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_Person_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_Person_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_CostCenter_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "MasterData_CostCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_ProcurementSection_ProcurementSectionId",
                        column: x => x.ProcurementSectionId,
                        principalTable: "MasterData_ProcurementSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_PurchaseGroup_PurchaseGroupId",
                        column: x => x.PurchaseGroupId,
                        principalTable: "MasterData_PurchaseGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_Person_RejectedBy",
                        column: x => x.RejectedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_Person_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PurchaseRequisition_MasterData_RequirmentPeriod_RequirmentPeriodId",
                        column: x => x.RequirmentPeriodId,
                        principalTable: "MasterData_RequirmentPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_PRDelegateTeam",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PersonId = table.Column<long>(nullable: true),
                    PurchaseRequisitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_PRDelegateTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_PRDelegateTeam_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PRDelegateTeam_Operational_PurchaseRequisition_PurchaseRequisitionId",
                        column: x => x.PurchaseRequisitionId,
                        principalTable: "Operational_PurchaseRequisition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_Project",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    ProjectCode = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    PlannedCompletionDate = table.Column<DateTime>(nullable: false),
                    IsBECMandatory = table.Column<bool>(nullable: false),
                    RequestType = table.Column<int>(nullable: false),
                    ProjectProcessType = table.Column<int>(nullable: false),
                    SourcingId = table.Column<long>(nullable: true),
                    HotelAccommodationId = table.Column<long>(nullable: true),
                    PurchaseRequisitionId = table.Column<long>(nullable: true),
                    AssignedPerson = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_Project_MasterData_Person_AssignedPerson",
                        column: x => x.AssignedPerson,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_Project_Operational_HotelAccommodationRequest_HotelAccommodationId",
                        column: x => x.HotelAccommodationId,
                        principalTable: "Operational_HotelAccommodationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_Project_Operational_PurchaseRequisition_PurchaseRequisitionId",
                        column: x => x.PurchaseRequisitionId,
                        principalTable: "Operational_PurchaseRequisition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_PRPRApprovers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    PersonId = table.Column<long>(nullable: true),
                    PurchaseRequisitionId = table.Column<long>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_PRPRApprovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_PRPRApprovers_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_PRPRApprovers_Operational_PurchaseRequisition_PurchaseRequisitionId",
                        column: x => x.PurchaseRequisitionId,
                        principalTable: "Operational_PurchaseRequisition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_FinancialEvaluation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    EvaluationName = table.Column<string>(nullable: true),
                    FinancialEvaluationValue = table.Column<double>(nullable: false),
                    AwardFactor = table.Column<int>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_FinancialEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_FinancialEvaluation_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_ProjectTeam",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<long>(nullable: true),
                    PersonId = table.Column<long>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_ProjectTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_ProjectTeam_MasterData_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operational_ProjectTeam_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operational_RequestForDocument",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    RequestDocumentType = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    AttachmentPath = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_RequestForDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_RequestForDocument_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_TechnicalEvaluation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    EvaluationName = table.Column<string>(nullable: true),
                    CutOffPoint = table.Column<double>(nullable: false),
                    TechnicalEvaluationValue = table.Column<double>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_TechnicalEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_TechnicalEvaluation_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_FinancialCriteriaGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Sum = table.Column<double>(nullable: false),
                    FinancialEvaluationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_FinancialCriteriaGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_FinancialCriteriaGroup_Operational_FinancialEvaluation_FinancialEvaluationId",
                        column: x => x.FinancialEvaluationId,
                        principalTable: "Operational_FinancialEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_RequestForDocAttachment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    AttachementPath = table.Column<string>(nullable: true),
                    RequestForDocumentationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_RequestForDocAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_RequestForDocAttachment_Operational_RequestForDocument_RequestForDocumentationId",
                        column: x => x.RequestForDocumentationId,
                        principalTable: "Operational_RequestForDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_RequestForDocumentApproval",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approver = table.Column<long>(nullable: false),
                    RequestForDocumentId = table.Column<long>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_RequestForDocumentApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_RequestForDocumentApproval_MasterData_Person_Approver",
                        column: x => x.Approver,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operational_RequestForDocumentApproval_Operational_RequestForDocument_RequestForDocumentId",
                        column: x => x.RequestForDocumentId,
                        principalTable: "Operational_RequestForDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_CriteriaGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    Sum = table.Column<double>(nullable: false),
                    TechnicalEvaluationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_CriteriaGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_CriteriaGroup_Operational_TechnicalEvaluation_TechnicalEvaluationId",
                        column: x => x.TechnicalEvaluationId,
                        principalTable: "Operational_TechnicalEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_FinancialCriteria",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    FinancialCriteriaGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_FinancialCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_FinancialCriteria_Operational_FinancialCriteriaGroup_FinancialCriteriaGroupId",
                        column: x => x.FinancialCriteriaGroupId,
                        principalTable: "Operational_FinancialCriteriaGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_Criterion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Measurment = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Necessity = table.Column<int>(nullable: false),
                    CriteriaGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_Criterion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_Criterion_Operational_CriteriaGroup_CriteriaGroupId",
                        column: x => x.CriteriaGroupId,
                        principalTable: "Operational_CriteriaGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_FinancialCriteriaItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    FiledName = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    FinancialCriteriaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_FinancialCriteriaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_FinancialCriteriaItem_Operational_FinancialCriteria_FinancialCriteriaId",
                        column: x => x.FinancialCriteriaId,
                        principalTable: "Operational_FinancialCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountSubscription_CompanyName",
                table: "AccountSubscription",
                column: "CompanyName",
                unique: true,
                filter: "[CompanyName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRolePrivilege_PrivilegeId",
                table: "ClientRolePrivilege",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRolePrivilege_RoleId",
                table: "ClientRolePrivilege",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_ClientRoleId",
                table: "ClientUser",
                column: "ClientRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserToken_UserId",
                table: "ClientUserToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_BusinessCategory_BusinessCategoryTypeId",
                table: "MasterData_BusinessCategory",
                column: "BusinessCategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_Office_CostCenterId",
                table: "MasterData_Office",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_Person_CostCenterId",
                table: "MasterData_Person",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_ProcurementSection_RequirmentPeriodId",
                table: "MasterData_ProcurementSection",
                column: "RequirmentPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_RequirmentPeriod_PurchaseGroupId",
                table: "MasterData_RequirmentPeriod",
                column: "PurchaseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_Station_CountryId",
                table: "MasterData_Station",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_Supplier_CountryId",
                table: "MasterData_Supplier",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_Supplier_VendorTypeId",
                table: "MasterData_Supplier",
                column: "VendorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_SupplierBusinessCategory_BusinessCategoryId",
                table: "MasterData_SupplierBusinessCategory",
                column: "BusinessCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterData_SupplierBusinessCategory_SupplierId",
                table: "MasterData_SupplierBusinessCategory",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_CriteriaGroup_TechnicalEvaluationId",
                table: "Operational_CriteriaGroup",
                column: "TechnicalEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_Criterion_CriteriaGroupId",
                table: "Operational_Criterion",
                column: "CriteriaGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_FinancialCriteria_FinancialCriteriaGroupId",
                table: "Operational_FinancialCriteria",
                column: "FinancialCriteriaGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_FinancialCriteriaGroup_FinancialEvaluationId",
                table: "Operational_FinancialCriteriaGroup",
                column: "FinancialEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_FinancialCriteriaItem_FinancialCriteriaId",
                table: "Operational_FinancialCriteriaItem",
                column: "FinancialCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_FinancialEvaluation_ProjectId",
                table: "Operational_FinancialEvaluation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_AssignedBy",
                table: "Operational_HotelAccommodationRequest",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_AssignedTo",
                table: "Operational_HotelAccommodationRequest",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_CostCenterId",
                table: "Operational_HotelAccommodationRequest",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_CountryId",
                table: "Operational_HotelAccommodationRequest",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_RejectedBy",
                table: "Operational_HotelAccommodationRequest",
                column: "RejectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_RequestedBy",
                table: "Operational_HotelAccommodationRequest",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationRequest_StationId",
                table: "Operational_HotelAccommodationRequest",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelARApprovers_HotelAccommodationId",
                table: "Operational_HotelARApprovers",
                column: "HotelAccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelARApprovers_PersonId",
                table: "Operational_HotelARApprovers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelARDelegateTeam_HotelAccommodationId",
                table: "Operational_HotelARDelegateTeam",
                column: "HotelAccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelARDelegateTeam_PersonId",
                table: "Operational_HotelARDelegateTeam",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PRDelegateTeam_PersonId",
                table: "Operational_PRDelegateTeam",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PRDelegateTeam_PurchaseRequisitionId",
                table: "Operational_PRDelegateTeam",
                column: "PurchaseRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_Project_AssignedPerson",
                table: "Operational_Project",
                column: "AssignedPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_Project_HotelAccommodationId",
                table: "Operational_Project",
                column: "HotelAccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_Project_PurchaseRequisitionId",
                table: "Operational_Project",
                column: "PurchaseRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ProjectTeam_PersonId",
                table: "Operational_ProjectTeam",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ProjectTeam_ProjectId",
                table: "Operational_ProjectTeam",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PRPRApprovers_PersonId",
                table: "Operational_PRPRApprovers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PRPRApprovers_PurchaseRequisitionId",
                table: "Operational_PRPRApprovers",
                column: "PurchaseRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_AssignedBy",
                table: "Operational_PurchaseRequisition",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_AssignedTo",
                table: "Operational_PurchaseRequisition",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_CostCenterId",
                table: "Operational_PurchaseRequisition",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_ProcurementSectionId",
                table: "Operational_PurchaseRequisition",
                column: "ProcurementSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_PurchaseGroupId",
                table: "Operational_PurchaseRequisition",
                column: "PurchaseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_RejectedBy",
                table: "Operational_PurchaseRequisition",
                column: "RejectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_RequestedBy",
                table: "Operational_PurchaseRequisition",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_PurchaseRequisition_RequirmentPeriodId",
                table: "Operational_PurchaseRequisition",
                column: "RequirmentPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocAttachment_RequestForDocumentationId",
                table: "Operational_RequestForDocAttachment",
                column: "RequestForDocumentationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocument_ProjectId",
                table: "Operational_RequestForDocument",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocumentApproval_Approver",
                table: "Operational_RequestForDocumentApproval",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocumentApproval_RequestForDocumentId",
                table: "Operational_RequestForDocumentApproval",
                column: "RequestForDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_TechnicalEvaluation_ProjectId",
                table: "Operational_TechnicalEvaluation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecovery_UserId",
                table: "PasswordRecovery",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Privilege_Action",
                table: "Privilege",
                column: "Action",
                unique: true,
                filter: "[Action] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Privilege_ClientUserId",
                table: "Privilege",
                column: "ClientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_AccountSubscriptionId",
                table: "Role",
                column: "AccountSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivilege_PrivilegeId",
                table: "RolePrivilege",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivilege_RoleId",
                table: "RolePrivilege",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountSubscriptionId",
                table: "User",
                column: "AccountSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PersonId",
                table: "User",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRolePrivilege");

            migrationBuilder.DropTable(
                name: "ClientUserToken");

            migrationBuilder.DropTable(
                name: "MasterData_Office");

            migrationBuilder.DropTable(
                name: "MasterData_SupplierBusinessCategory");

            migrationBuilder.DropTable(
                name: "MasterDataTransactionalHistory");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Operational_Criterion");

            migrationBuilder.DropTable(
                name: "Operational_FinancialCriteriaItem");

            migrationBuilder.DropTable(
                name: "Operational_HotelARApprovers");

            migrationBuilder.DropTable(
                name: "Operational_HotelARDelegateTeam");

            migrationBuilder.DropTable(
                name: "Operational_PRDelegateTeam");

            migrationBuilder.DropTable(
                name: "Operational_ProjectTeam");

            migrationBuilder.DropTable(
                name: "Operational_PRPRApprovers");

            migrationBuilder.DropTable(
                name: "Operational_RequestForDocAttachment");

            migrationBuilder.DropTable(
                name: "Operational_RequestForDocumentApproval");

            migrationBuilder.DropTable(
                name: "PasswordRecovery");

            migrationBuilder.DropTable(
                name: "RolePrivilege");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "ClientPrivilege");

            migrationBuilder.DropTable(
                name: "MasterData_BusinessCategory");

            migrationBuilder.DropTable(
                name: "MasterData_Supplier");

            migrationBuilder.DropTable(
                name: "Operational_CriteriaGroup");

            migrationBuilder.DropTable(
                name: "Operational_FinancialCriteria");

            migrationBuilder.DropTable(
                name: "Operational_RequestForDocument");

            migrationBuilder.DropTable(
                name: "Privilege");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "MasterData_BusinessCategoryType");

            migrationBuilder.DropTable(
                name: "MasterData_VendorType");

            migrationBuilder.DropTable(
                name: "Operational_TechnicalEvaluation");

            migrationBuilder.DropTable(
                name: "Operational_FinancialCriteriaGroup");

            migrationBuilder.DropTable(
                name: "ClientUser");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Operational_FinancialEvaluation");

            migrationBuilder.DropTable(
                name: "ClientRole");

            migrationBuilder.DropTable(
                name: "AccountSubscription");

            migrationBuilder.DropTable(
                name: "Operational_Project");

            migrationBuilder.DropTable(
                name: "Operational_HotelAccommodationRequest");

            migrationBuilder.DropTable(
                name: "Operational_PurchaseRequisition");

            migrationBuilder.DropTable(
                name: "MasterData_Station");

            migrationBuilder.DropTable(
                name: "MasterData_Person");

            migrationBuilder.DropTable(
                name: "MasterData_ProcurementSection");

            migrationBuilder.DropTable(
                name: "MasterData_Country");

            migrationBuilder.DropTable(
                name: "MasterData_CostCenter");

            migrationBuilder.DropTable(
                name: "MasterData_RequirmentPeriod");

            migrationBuilder.DropTable(
                name: "MasterData_PurchaseGroup");
        }
    }
}
