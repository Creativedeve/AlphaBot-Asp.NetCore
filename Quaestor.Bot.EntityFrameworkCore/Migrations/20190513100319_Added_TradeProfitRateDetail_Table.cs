using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_TradeProfitRateDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeProfitRateDetail",
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
                    BTCInvested = table.Column<decimal>(type: "decimal(18,10)", nullable: true),
                    CurrencyPurchased = table.Column<decimal>(type: "decimal(18,10)", nullable: true),
                    AverageCurrencyRate = table.Column<decimal>(type: "decimal(18,10)", nullable: true),
                    TradeProfitPercentageRate = table.Column<decimal>(type: "decimal(18,10)", nullable: true),
                    TradeProfitSaleRate = table.Column<decimal>(type: "decimal(18,10)", nullable: true),
                    TradeProfitRateId = table.Column<int>(nullable: false),
                    TillCount = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeProfitRateDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeProfitRateDetail_TradeProfitRate_TradeProfitRateId",
                        column: x => x.TradeProfitRateId,
                        principalTable: "TradeProfitRate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeProfitRateDetail_TradeProfitRateId",
                table: "TradeProfitRateDetail",
                column: "TradeProfitRateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeProfitRateDetail");
        }
    }
}
