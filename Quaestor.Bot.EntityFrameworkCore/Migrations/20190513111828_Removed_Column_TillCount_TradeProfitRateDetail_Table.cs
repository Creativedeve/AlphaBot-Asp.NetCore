using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Removed_Column_TillCount_TradeProfitRateDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TillCount",
                table: "TradeProfitRateDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TillCount",
                table: "TradeProfitRateDetail",
                nullable: false,
                defaultValue: 0);
        }
    }
}
