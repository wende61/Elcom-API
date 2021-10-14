using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class removeAccountSuvScription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_AccountSubscription_AccountSubscriptionId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_AccountSubscriptionId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "AccountSubscriptionId",
                table: "Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountSubscriptionId",
                table: "Role",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Role_AccountSubscriptionId",
                table: "Role",
                column: "AccountSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_AccountSubscription_AccountSubscriptionId",
                table: "Role",
                column: "AccountSubscriptionId",
                principalTable: "AccountSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
