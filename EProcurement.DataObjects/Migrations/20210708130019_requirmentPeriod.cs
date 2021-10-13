using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class requirmentPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequirementPeriodId",
                table: "Operational_PurchaseRequisition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RequirementPeriodId",
                table: "Operational_PurchaseRequisition",
                type: "bigint",
                nullable: true);
        }
    }
}
