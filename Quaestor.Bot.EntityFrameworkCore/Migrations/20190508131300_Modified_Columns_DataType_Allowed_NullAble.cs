using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Modified_Columns_DataType_Allowed_NullAble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo");

            migrationBuilder.AlterColumn<int>(
                name: "UserSessionDetailId",
                table: "PreserveBuyInfo",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ReBuySequence",
                table: "PreserveBuyInfo",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId",
                principalTable: "PurchaseOrderRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo");

            migrationBuilder.AlterColumn<int>(
                name: "UserSessionDetailId",
                table: "PreserveBuyInfo",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReBuySequence",
                table: "PreserveBuyInfo",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PreserveBuyInfo_PurchaseOrderRecord_PurchaseOrderRecordId",
                table: "PreserveBuyInfo",
                column: "PurchaseOrderRecordId",
                principalTable: "PurchaseOrderRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
