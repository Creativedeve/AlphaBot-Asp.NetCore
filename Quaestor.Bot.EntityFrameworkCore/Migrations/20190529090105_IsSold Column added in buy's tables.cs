using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class IsSoldColumnaddedinbuystables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "TradeProfitRate",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "PurchaseOrderRecordDetail",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "PurchaseOrderRecord",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "TradeProfitRate");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "PurchaseOrderRecordDetail");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "PurchaseOrderRecord");
        }
    }
}
