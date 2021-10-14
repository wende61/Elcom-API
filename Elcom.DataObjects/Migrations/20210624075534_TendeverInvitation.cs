using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class TendeverInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_TenderInvitation",
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
                    Description = table.Column<string>(nullable: true),
                    Announcemnet = table.Column<string>(nullable: true),
                    ResponseDueDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_TenderInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_TenderInvitation_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_SupplierTenderInvitation",
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
                    TenderInvitationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_SupplierTenderInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_SupplierTenderInvitation_MasterData_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "MasterData_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operational_SupplierTenderInvitation_Operational_TenderInvitation_TenderInvitationId",
                        column: x => x.TenderInvitationId,
                        principalTable: "Operational_TenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_SupplierTenderInvitation_SupplierId",
                table: "Operational_SupplierTenderInvitation",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_SupplierTenderInvitation_TenderInvitationId",
                table: "Operational_SupplierTenderInvitation",
                column: "TenderInvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_TenderInvitation_ProjectId",
                table: "Operational_TenderInvitation",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operational_SupplierTenderInvitation");

            migrationBuilder.DropTable(
                name: "Operational_TenderInvitation");
        }
    }
}
