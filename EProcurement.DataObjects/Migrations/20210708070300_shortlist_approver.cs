using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class shortlist_approver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operational_ShortListApproval_MasterData_Person_Approver",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_Operational_ShortListApproval_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropIndex(
                name: "IX_Operational_ShortListApproval_Approver",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropIndex(
                name: "IX_Operational_ShortListApproval_SupplierTenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropColumn(
                name: "Approver",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropColumn(
                name: "IsApprover",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropColumn(
                name: "SupplierTenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "Operational_ShortListApproval",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TenderInvitationId",
                table: "Operational_ShortListApproval",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ShortListApprovers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approver = table.Column<long>(nullable: false),
                    ShortListApprovalId = table.Column<long>(nullable: false),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    IsApprover = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListApprovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListApprovers_MasterData_Person_Approver",
                        column: x => x.Approver,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShortListApprovers_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortListedSuppliers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<long>(nullable: false),
                    ShortListApprovalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListedSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListedSuppliers_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShortListedSuppliers_MasterData_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "MasterData_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_PersonId",
                table: "Operational_ShortListApproval",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_TenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "TenderInvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListApprovers_Approver",
                table: "ShortListApprovers",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListApprovers_ShortListApprovalId",
                table: "ShortListApprovers",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSuppliers_ShortListApprovalId",
                table: "ShortListedSuppliers",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSuppliers_SupplierId",
                table: "ShortListedSuppliers",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operational_ShortListApproval_MasterData_Person_PersonId",
                table: "Operational_ShortListApproval",
                column: "PersonId",
                principalTable: "MasterData_Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operational_ShortListApproval_Operational_TenderInvitation_TenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "TenderInvitationId",
                principalTable: "Operational_TenderInvitation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operational_ShortListApproval_MasterData_Person_PersonId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropForeignKey(
                name: "FK_Operational_ShortListApproval_Operational_TenderInvitation_TenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropTable(
                name: "ShortListApprovers");

            migrationBuilder.DropTable(
                name: "ShortListedSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_Operational_ShortListApproval_PersonId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropIndex(
                name: "IX_Operational_ShortListApproval_TenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Operational_ShortListApproval");

            migrationBuilder.DropColumn(
                name: "TenderInvitationId",
                table: "Operational_ShortListApproval");

            migrationBuilder.AddColumn<long>(
                name: "Approver",
                table: "Operational_ShortListApproval",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprover",
                table: "Operational_ShortListApproval",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SupplierTenderInvitationId",
                table: "Operational_ShortListApproval",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_Approver",
                table: "Operational_ShortListApproval",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_SupplierTenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "SupplierTenderInvitationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operational_ShortListApproval_MasterData_Person_Approver",
                table: "Operational_ShortListApproval",
                column: "Approver",
                principalTable: "MasterData_Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operational_ShortListApproval_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "SupplierTenderInvitationId",
                principalTable: "Operational_SupplierTenderInvitation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
