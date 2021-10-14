using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class is_instantiated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isInitiated",
                table: "Operational_PurchaseRequisition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInitiated",
                table: "Operational_HotelAccommodationRequest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isInitiated",
                table: "Operational_PurchaseRequisition");

            migrationBuilder.DropColumn(
                name: "isInitiated",
                table: "Operational_HotelAccommodationRequest");
        }
    }
}
