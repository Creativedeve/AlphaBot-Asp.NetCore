using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_UserId_IN_UserKey_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserKeys",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserKeys_UserId",
                table: "UserKeys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserKeys_AbpUsers_UserId",
                table: "UserKeys",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserKeys_AbpUsers_UserId",
                table: "UserKeys");

            migrationBuilder.DropIndex(
                name: "IX_UserKeys_UserId",
                table: "UserKeys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserKeys");
        }
    }
}
