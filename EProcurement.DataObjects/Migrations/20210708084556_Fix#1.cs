using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EProcurement.DataObjects.Migrations
{
    public partial class Fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortListApprovers");

            migrationBuilder.DropTable(
                name: "ShortListedSuppliers");

            migrationBuilder.DropTable(
                name: "Operational_ShortListApproval");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operational_ShortListApproval",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false),
                    RegisteredBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenderInvitationId = table.Column<long>(type: "bigint", nullable: false),
                    TimeZoneInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "ShortListApprovers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    Approver = table.Column<long>(type: "bigint", nullable: true),
                    IsApprover = table.Column<bool>(type: "bit", nullable: false),
                    ShortListApprovalId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListApprovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListApprovers_MasterData_Person_Approver",
                        column: x => x.Approver,
                        principalTable: "MasterData_Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShortListApprovers_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShortListedSuppliers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false),
                    RegisteredBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortListApprovalId = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierTenderInvitationId = table.Column<long>(type: "bigint", nullable: true),
                    TimeZoneInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortListedSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortListedSuppliers_Operational_ShortListApproval_ShortListApprovalId",
                        column: x => x.ShortListApprovalId,
                        principalTable: "Operational_ShortListApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShortListedSuppliers_Operational_SupplierTenderInvitation_SupplierTenderInvitationId",
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
                name: "IX_ShortListApprovers_Approver",
                table: "ShortListApprovers",
                column: "Approver");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListApprovers_ShortListApprovalId",
                table: "ShortListApprovers",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSuppliers_ShortListApprovalId",
                table: "ShortListedSuppliers",
                column: "ShortListApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortListedSuppliers_SupplierTenderInvitationId",
                table: "ShortListedSuppliers",
                column: "SupplierTenderInvitationId");
        }
    }
}
