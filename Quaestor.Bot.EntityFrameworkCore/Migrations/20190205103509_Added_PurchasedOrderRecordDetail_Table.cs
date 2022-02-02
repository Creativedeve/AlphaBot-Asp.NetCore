using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_PurchasedOrderRecordDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderRecordDetail",
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
                    ReBuySequence = table.Column<int>(nullable: false),
                    ReBuyRate = table.Column<decimal>(nullable: true),
                    ReBuyValue = table.Column<decimal>(nullable: true),
                    ReBuyWith = table.Column<decimal>(nullable: true),
                    PurchaseOrderRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderRecordDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderRecordDetail_PurchaseOrderRecord_PurchaseOrderRecordId",
                        column: x => x.PurchaseOrderRecordId,
                        principalTable: "PurchaseOrderRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRecordDetail_PurchaseOrderRecordId",
                table: "PurchaseOrderRecordDetail",
                column: "PurchaseOrderRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderRecordDetail");
        }
    }
}
