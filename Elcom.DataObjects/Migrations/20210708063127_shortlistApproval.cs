using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class shortlistApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_ShortListApproval",
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
                    Approver = table.Column<long>(nullable: false),
                    SupplierTenderInvitationId = table.Column<long>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    IsApprover = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_ShortListApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_ShortListApproval_MasterData_Person_Approver",
                        column: x => x.Approver,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operational_ShortListApproval_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                        column: x => x.SupplierTenderInvitationId,
                        principalTable: "Operational_SupplierTenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_Approver",
                table: "Operational_ShortListApproval",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_SupplierTenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "SupplierTenderInvitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operational_ShortListApproval");
        }
    }
}
