using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Elcom.DataObjects.Migrations
{
    public partial class HotelAccomodationCriteria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_HotelAccommodationCriteria",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    DailyRoomNumber = table.Column<int>(nullable: false),
                    WeeklyFrequency = table.Column<int>(nullable: false),
                    YearlyFrequency = table.Column<int>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_HotelAccommodationCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_HotelAccommodationCriteria_Operational_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Operational_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_HotelAccommodationCriteria_ProjectId",
                table: "Operational_HotelAccommodationCriteria",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operational_HotelAccommodationCriteria");
        }
    }
}
