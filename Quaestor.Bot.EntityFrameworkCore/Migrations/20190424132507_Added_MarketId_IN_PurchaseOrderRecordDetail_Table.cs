using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_MarketId_IN_PurchaseOrderRecordDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarketId",
                table: "PurchaseOrderRecordDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderRecordDetail_MarketId",
                table: "PurchaseOrderRecordDetail",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderRecordDetail_Markets_MarketId",
                table: "PurchaseOrderRecordDetail",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderRecordDetail_Markets_MarketId",
                table: "PurchaseOrderRecordDetail");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderRecordDetail_MarketId",
                table: "PurchaseOrderRecordDetail");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "PurchaseOrderRecordDetail");
        }
    }
}
