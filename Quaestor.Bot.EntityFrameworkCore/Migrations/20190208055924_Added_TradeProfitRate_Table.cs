using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_TradeProfitRate_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeProfitRate",
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
                    BTCInvested = table.Column<decimal>(nullable: true),
                    CurrencyPurchased = table.Column<decimal>(nullable: true),
                    AverageCurrencyRate = table.Column<decimal>(nullable: true),
                    TradeProfitPercentageRate = table.Column<decimal>(nullable: true),
                    TradeProfitSaleRate = table.Column<decimal>(nullable: true),
                    PurchaseOrderRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeProfitRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeProfitRate_PurchaseOrderRecord_PurchaseOrderRecordId",
                        column: x => x.PurchaseOrderRecordId,
                        principalTable: "PurchaseOrderRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeProfitRate_PurchaseOrderRecordId",
                table: "TradeProfitRate",
                column: "PurchaseOrderRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeProfitRate");
        }
    }
}
