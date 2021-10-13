using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class supplierTenderProposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_SuppliersTenderProposal",
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
                    ShortListedSupplierId = table.Column<long>(nullable: false),
                    SupplierTenderInvitationId = table.Column<long>(nullable: false),
                    SubmitionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_SuppliersTenderProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_SuppliersTenderProposal_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                        column: x => x.SupplierTenderInvitationId,
                        principalTable: "Operational_SupplierTenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operational_SuppliersProposalAttachment",
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
                    FilePath = table.Column<string>(nullable: true),
                    Seen = table.Column<bool>(nullable: false),
                    SeenDate = table.Column<DateTime>(nullable: true),
                    DocumentType = table.Column<int>(nullable: false),
                    SuppliersTenderProposalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_SuppliersProposalAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_SuppliersProposalAttachment_Operational_SuppliersTenderProposal_SuppliersTenderProposalId",
                        column: x => x.SuppliersTenderProposalId,
                        principalTable: "Operational_SuppliersTenderProposal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_SuppliersProposalAttachment_SuppliersTenderProposalId",
                table: "Operational_SuppliersProposalAttachment",
                column: "SuppliersTenderProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Operational_SuppliersTenderProposal_SupplierTenderInvitationId",
                table: "Operational_SuppliersTenderProposal",
                column: "SupplierTenderInvitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operational_SuppliersProposalAttachment");

            migrationBuilder.DropTable(
                name: "Operational_SuppliersTenderProposal");
        }
    }
}
