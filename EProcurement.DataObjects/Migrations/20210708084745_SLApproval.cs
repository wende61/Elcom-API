using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class SLApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_ShortListApproval",
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
                    ApprovalStatus = table.Column<int>(nullable: false),
                    TenderInvitationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operational_ShortListApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operational_ShortListApproval_Operational_TenderInvitation_TenderInvitationId",
                        column: x => x.TenderInvitationId,
                        principalTable: "Operational_TenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortListApprover",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approver = table.Column<long>(nullable: true),
                    ShortListApprovalId = table.Column<long>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    IsApprover = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListApprover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListApprover_MasterData_Person_Approver",
                        column: x => x.Approver,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShortListApprover_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShortListedSupplier",
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
                    SupplierTenderInvitationId = table.Column<long>(nullable: true),
                    ShortListApprovalId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListedSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListedSupplier_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShortListedSupplier_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
                        column: x => x.SupplierTenderInvitationId,
                        principalTable: "Operational_SupplierTenderInvitation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operational_ShortListApproval_TenderInvitationId",
                table: "Operational_ShortListApproval",
                column: "TenderInvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListApprover_Approver",
                table: "ShortListApprover",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListApprover_ShortListApprovalId",
                table: "ShortListApprover",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSupplier_ShortListApprovalId",
                table: "ShortListedSupplier",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSupplier_SupplierTenderInvitationId",
                table: "ShortListedSupplier",
                column: "SupplierTenderInvitationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortListApprover");

            migrationBuilder.DropTable(
                name: "ShortListedSupplier");

            migrationBuilder.DropTable(
                name: "Operational_ShortListApproval");
        }
    }
}
