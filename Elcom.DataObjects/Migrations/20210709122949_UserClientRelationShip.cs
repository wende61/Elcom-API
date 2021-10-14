using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class UserClientRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ClientId",
                table: "User",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ClientUser_ClientId",
                table: "User",
                column: "ClientId",
                principalTable: "ClientUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ClientUser_ClientId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ClientId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "User");
        }
    }
}
