using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_DropByColumns_IN_UserSessionDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSessionDetail_Markets_MarketId",
                table: "UserSessionDetail");

            migrationBuilder.DropIndex(
                name: "IX_UserSessionDetail_MarketId",
                table: "UserSessionDetail");

            migrationBuilder.AddColumn<decimal?>(
                name: "FifthRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "FirstRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "FourthRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "SecondRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "SeventhRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "SixthRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);

            migrationBuilder.AddColumn<decimal?>(
                name: "ThirdRebuyDrop",
                table: "UserSessionDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FifthRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "FirstRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "FourthRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "SecondRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "SeventhRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "SixthRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.DropColumn(
                name: "ThirdRebuyDrop",
                table: "UserSessionDetail");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionDetail_MarketId",
                table: "UserSessionDetail",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessionDetail_Markets_MarketId",
                table: "UserSessionDetail",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
