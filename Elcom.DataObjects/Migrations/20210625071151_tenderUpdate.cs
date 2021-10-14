using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class tenderUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Announcemnet",
                table: "Operational_TenderInvitation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Announcemnet",
                table: "Operational_TenderInvitation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
