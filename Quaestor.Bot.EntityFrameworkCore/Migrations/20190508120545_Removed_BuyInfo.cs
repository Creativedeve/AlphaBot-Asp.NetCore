using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Removed_BuyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreserveBuyInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreserveBuyInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuyType = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    MarketId = table.Column<int>(nullable: false),
                    PurchaseOrderRecordId = table.Column<int>(nullable: true),
                    ReBuySequence = table.Column<int>(nullable: false),
                    UserSessionDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreserveBuyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                        column: x => x.PurchaseOrderRecordId,
                        principalTable: "PurchaseOrderRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreserveBuyInfo_UserSessionDetail_UserSessionDetailId",
                        column: x => x.UserSessionDetailId,
                        principalTable: "UserSessionDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PreserveBuyInfo_UserSessionDetailId",
                table: "PreserveBuyInfo",
                column: "UserSessionDetailId");
        }
    }
}
