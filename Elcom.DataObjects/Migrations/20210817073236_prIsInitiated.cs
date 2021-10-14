using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class prIsInitiated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isInitiated",
                table: "Operational_PurchaseRequisition",
                newName: "IsInitiated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsInitiated",
                table: "Operational_PurchaseRequisition",
                newName: "isInitiated");
        }
    }
}
