using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class DeletSexFromPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "MasterData_Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "MasterData_Person",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
