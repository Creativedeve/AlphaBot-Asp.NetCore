using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_PurchaseOrderRecordId_IN_BuyInfo_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId",
                principalTable: "PurchaseOrderRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo");

            migrationBuilder.DropIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderRecordId",
                table: "PreserveBuyInfo");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderRecordId",
                table: "PreserveBuyInfo");
        }
    }
}
