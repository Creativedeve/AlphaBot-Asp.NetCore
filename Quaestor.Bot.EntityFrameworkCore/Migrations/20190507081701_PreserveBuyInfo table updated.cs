using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class PreserveBuyInfotableupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderDetailRecordId",
                table: "PreserveBuyInfo");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderDetailRecordId",
                table: "PreserveBuyInfo",
                newName: "UserSessionDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_PreserveBuyInfo_PurchaseOrderDetailRecordId",
                table: "PreserveBuyInfo",
                newName: "IX_PreserveBuyInfo_UserSessionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreserveBuyInfo_UserSessionDetail_UserSessionDetailId",
                table: "PreserveBuyInfo",
                column: "UserSessionDetailId",
                principalTable: "UserSessionDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreserveBuyInfo_UserSessionDetail_UserSessionDetailId",
                table: "PreserveBuyInfo");

            migrationBuilder.RenameColumn(
                name: "UserSessionDetailId",
                table: "PreserveBuyInfo",
                newName: "PurchaseOrderDetailRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_PreserveBuyInfo_UserSessionDetailId",
                table: "PreserveBuyInfo",
                newName: "IX_PreserveBuyInfo_PurchaseOrderDetailRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderDetailRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderDetailRecordId",
                principalTable: "PurchaseOrderRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
