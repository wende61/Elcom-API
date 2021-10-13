using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class invitationFloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RecordStatus",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredBy",
                table: "Operational_TenderInvitationFloat",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Operational_TenderInvitationFloat",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneInfo",
                table: "Operational_TenderInvitationFloat",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Operational_TenderInvitationFloat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "RegisteredBy",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "TimeZoneInfo",
                table: "Operational_TenderInvitationFloat");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Operational_TenderInvitationFloat");
        }
    }
}
