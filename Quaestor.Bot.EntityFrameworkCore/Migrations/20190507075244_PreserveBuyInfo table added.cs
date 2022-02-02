using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class PreserveBuyInfotableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreserveBuyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PurchaseOrderRecordId = table.Column<int>(nullable: true),
                    PurchaseOrderDetailRecordId = table.Column<int>(nullable: true),
                    BuyType = table.Column<string>(nullable: true),
                    IsProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreserveBuyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderDetailRecordId",
                        column: x => x.PurchaseOrderDetailRecordId,
                        principalTable: "PurchaseOrderRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                        column: x => x.PurchaseOrderRecordId,
                        principalTable: "PurchaseOrderRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderDetailRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderDetailRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreserveBuyInfo");
        }
    }
}
