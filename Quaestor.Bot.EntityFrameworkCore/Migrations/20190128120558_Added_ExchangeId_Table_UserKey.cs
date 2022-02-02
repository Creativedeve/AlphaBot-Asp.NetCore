using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_ExchangeId_Table_UserKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExchangeId",
                table: "UserKeys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserKeys_ExchangeId",
                table: "UserKeys",
                column: "ExchangeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserKeys_Exchange_ExchangeId",
                table: "UserKeys",
                column: "ExchangeId",
                principalTable: "Exchange",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserKeys_Exchange_ExchangeId",
                table: "UserKeys");

            migrationBuilder.DropIndex(
                name: "IX_UserKeys_ExchangeId",
                table: "UserKeys");

            migrationBuilder.DropColumn(
                name: "ExchangeId",
                table: "UserKeys");
        }
    }
}
