using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_TradeProfitSellInfo_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeProfitSellInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    SellQuantity = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    SellRate = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    SellAmount = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    OrderId = table.Column<long>(nullable: false),
                    TradeProfitRateId = table.Column<int>(nullable: false),
                    MarketId = table.Column<int>(nullable: false),
                    PurchaseOrderRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeProfitSellInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeProfitSellInfo");
        }
    }
}
