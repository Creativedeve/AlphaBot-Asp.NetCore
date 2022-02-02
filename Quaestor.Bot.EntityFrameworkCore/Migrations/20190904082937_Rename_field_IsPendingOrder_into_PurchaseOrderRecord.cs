using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Rename_field_IsPendingOrder_into_PurchaseOrderRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "PurchaseOrderRecord",
                newName: "IsPendingOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPendingOrder",
                table: "PurchaseOrderRecord",
                newName: "OrderStatus");
        }
    }
}
