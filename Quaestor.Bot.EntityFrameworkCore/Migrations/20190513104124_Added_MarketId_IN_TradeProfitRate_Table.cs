using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_MarketId_IN_TradeProfitRate_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarketId",
                table: "TradeProfitRate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TradeProfitRate_MarketId",
                table: "TradeProfitRate",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeProfitRate_Markets_MarketId",
                table: "TradeProfitRate",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeProfitRate_Markets_MarketId",
                table: "TradeProfitRate");

            migrationBuilder.DropIndex(
                name: "IX_TradeProfitRate_MarketId",
                table: "TradeProfitRate");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "TradeProfitRate");
        }
    }
}
