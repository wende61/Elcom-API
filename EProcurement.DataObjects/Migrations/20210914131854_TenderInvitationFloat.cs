using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class TenderInvitationFloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_TenderInvitationFloat",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderInvitationId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_TenderInvitationFloat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_TenderInvitationFloat_Operational_TenderInvitation_TenderInvitationId",
                        column: x => x.TenderInvitationId,
                        principalTable: "Operational_TenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_TenderInvitationFloat_TenderInvitationId",
                table: "Operational_TenderInvitationFloat",
                column: "TenderInvitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operational_TenderInvitationFloat");
        }
    }
}
