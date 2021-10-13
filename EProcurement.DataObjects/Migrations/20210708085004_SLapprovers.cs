using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class SLapprovers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortListApprover_MasterData_Person_Approver",
                table: "ShortListApprover");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortListApprover_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListApprover");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortListApprover",
                table: "ShortListApprover");

            migrationBuilder.RenameTable(
                name: "ShortListApprover",
                newName: "ShortListApprovers");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListApprover_ShortListApprovalId",
                table: "ShortListApprovers",
                newName: "IX_ShortListApprovers_ShortListApprovalId");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListApprover_Approver",
                table: "ShortListApprovers",
                newName: "IX_ShortListApprovers_Approver");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortListApprovers",
                table: "ShortListApprovers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListApprovers_MasterData_Person_Approver",
                table: "ShortListApprovers",
                column: "Approver",
                principalTable: "MasterData_Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListApprovers_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListApprovers",
                column: "ShortListApprovalId",
                principalTable: "Operational_ShortListApproval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortListApprovers_MasterData_Person_Approver",
                table: "ShortListApprovers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortListApprovers_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListApprovers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortListApprovers",
                table: "ShortListApprovers");

            migrationBuilder.RenameTable(
                name: "ShortListApprovers",
                newName: "ShortListApprover");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListApprovers_ShortListApprovalId",
                table: "ShortListApprover",
                newName: "IX_ShortListApprover_ShortListApprovalId");

            migrationBuilder.RenameIndex(
                name: "IX_ShortListApprovers_Approver",
                table: "ShortListApprover",
                newName: "IX_ShortListApprover_Approver");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortListApprover",
                table: "ShortListApprover",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListApprover_MasterData_Person_Approver",
                table: "ShortListApprover",
                column: "Approver",
                principalTable: "MasterData_Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortListApprover_Operational_ShortListApproval_ShortListApprovalId",
                table: "ShortListApprover",
                column: "ShortListApprovalId",
                principalTable: "Operational_ShortListApproval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
