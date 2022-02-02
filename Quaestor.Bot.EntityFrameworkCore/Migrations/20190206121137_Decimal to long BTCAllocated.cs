using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class DecimaltolongBTCAllocated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BTCAllocated",
                table: "UserSessionDetail",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BTCAllocated",
                table: "UserSessionDetail",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
