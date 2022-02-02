using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_Column_ErrorType_UserSessionDetail_PurchaseOrderDetailId_ErrorLog_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorType",
                table: "ErrorLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderDetailId",
                table: "ErrorLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ErrorLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserSessionDetailId",
                table: "ErrorLogs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorType",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderDetailId",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ErrorLogs");

            migrationBuilder.DropColumn(
                name: "UserSessionDetailId",
                table: "ErrorLogs");
        }
    }
}
