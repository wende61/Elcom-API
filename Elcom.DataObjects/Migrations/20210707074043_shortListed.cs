using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class shortListed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShortListed",
                table: "Operational_SupplierTenderInvitation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortListed",
                table: "Operational_SupplierTenderInvitation");
        }
    }
}
