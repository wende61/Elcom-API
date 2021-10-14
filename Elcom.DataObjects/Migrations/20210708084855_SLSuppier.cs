using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class SLSuppier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortListedSupplier_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListedSupplier");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortListedSupplier_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "ShortListedSupplier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortListedSupplier",
                table: "ShortListedSupplier");

            migrationBuilder.RenameTable(
                name: "ShortListedSupplier",
                newName: "ShortListedSuppliers");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListedSupplier_SupplierTenderInvitationId",
                table: "ShortListedSuppliers",
                newName: "IX_ShortListedSuppliers_SupplierTenderInvitationId");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListedSupplier_ShortListApprovalId",
                table: "ShortListedSuppliers",
                newName: "IX_ShortListedSuppliers_ShortListApprovalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortListedSuppliers",
                table: "ShortListedSuppliers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListedSuppliers_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListedSuppliers",
                column: "ShortListApprovalId",
                principalTable: "Operational_ShortListApproval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListedSuppliers_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "ShortListedSuppliers",
                column: "SupplierTenderInvitationId",
                principalTable: "Operational_SupplierTenderInvitation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortListedSuppliers_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListedSuppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortListedSuppliers_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "ShortListedSuppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortListedSuppliers",
                table: "ShortListedSuppliers");

            migrationBuilder.RenameTable(
                name: "ShortListedSuppliers",
                newName: "ShortListedSupplier");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListedSuppliers_SupplierTenderInvitationId",
                table: "ShortListedSupplier",
                newName: "IX_ShortListedSupplier_SupplierTenderInvitationId");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListedSuppliers_ShortListApprovalId",
                table: "ShortListedSupplier",
                newName: "IX_ShortListedSupplier_ShortListApprovalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortListedSupplier",
                table: "ShortListedSupplier",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListedSupplier_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListedSupplier",
                column: "ShortListApprovalId",
                principalTable: "Operational_ShortListApproval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListedSupplier_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                table: "ShortListedSupplier",
                column: "SupplierTenderInvitationId",
                principalTable: "Operational_SupplierTenderInvitation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
