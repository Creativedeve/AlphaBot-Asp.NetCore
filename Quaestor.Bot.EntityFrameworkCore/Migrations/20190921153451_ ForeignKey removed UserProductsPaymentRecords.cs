using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class ForeignKeyremovedUserProductsPaymentRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProductsPaymentRecords_UserProducts_UserProductId",
                table: "UserProductsPaymentRecords");

            migrationBuilder.DropIndex(
                name: "IX_UserProductsPaymentRecords_UserProductId",
                table: "UserProductsPaymentRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProductsPaymentRecords_UserProductId",
                table: "UserProductsPaymentRecords",
                column: "UserProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProductsPaymentRecords_UserProducts_UserProductId",
                table: "UserProductsPaymentRecords",
                column: "UserProductId",
                principalTable: "UserProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
