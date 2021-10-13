using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class stringchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BidInterest",
                table: "Operational_SupplierTenderInvitation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Operational_SupplierTenderInvitation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "Operational_SupplierTenderInvitation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidInterest",
                table: "Operational_SupplierTenderInvitation");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Operational_SupplierTenderInvitation");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "Operational_SupplierTenderInvitation");
        }
    }
}
