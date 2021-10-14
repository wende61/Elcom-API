using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class BidClosign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_SupplierId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Operational_RequestForDocument_ProjectId",
                table: "Operational_RequestForDocument");

            migrationBuilder.AddColumn<DateTime>(
                name: "BidClosingDate",
                table: "Operational_Project",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TechnicalOpeningDate",
                table: "Operational_Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SupplierId",
                table: "User",
                column: "SupplierId",
                unique: true,
                filter: "[SupplierId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocument_ProjectId",
                table: "Operational_RequestForDocument",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_SupplierId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Operational_RequestForDocument_ProjectId",
                table: "Operational_RequestForDocument");

            migrationBuilder.DropColumn(
                name: "BidClosingDate",
                table: "Operational_Project");

            migrationBuilder.DropColumn(
                name: "TechnicalOpeningDate",
                table: "Operational_Project");

            migrationBuilder.CreateIndex(
                name: "IX_User_SupplierId",
                table: "User",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_RequestForDocument_ProjectId",
                table: "Operational_RequestForDocument",
                column: "ProjectId");
        }
    }
}
