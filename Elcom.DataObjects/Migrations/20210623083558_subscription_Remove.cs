using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class subscription_Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AccountSubscription_AccountSubscriptionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AccountSubscriptionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccountSubscriptionId",
                table: "User");

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SupplierId",
                table: "User",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_MasterData_Supplier_SupplierId",
                table: "User",
                column: "SupplierId",
                principalTable: "MasterData_Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_MasterData_Supplier_SupplierId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SupplierId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "User");

            migrationBuilder.AddColumn<long>(
                name: "AccountSubscriptionId",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountSubscriptionId",
                table: "User",
                column: "AccountSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AccountSubscription_AccountSubscriptionId",
                table: "User",
                column: "AccountSubscriptionId",
                principalTable: "AccountSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
