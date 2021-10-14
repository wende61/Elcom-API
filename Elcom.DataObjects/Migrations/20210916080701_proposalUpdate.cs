using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class proposalUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortListedSupplierId",
                table: "Operational_SuppliersTenderProposal");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ShortListedSupplierId",
                table: "Operational_SuppliersTenderProposal",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
