using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class paymenttablenamechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProductsPaymentReccords");

            migrationBuilder.CreateTable(
                name: "UserProductsPaymentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    UserProductId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    SourceCurrency = table.Column<string>(nullable: true),
                    SourceAmount = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    DestinationCurrency = table.Column<string>(nullable: true),
                    DestinationAmount = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    ActualAmountPaid = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ExpiresAt = table.Column<DateTime>(nullable: false),
                    ConfirmedAt = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProductsPaymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProductsPaymentRecords_UserProducts_UserProductId",
                        column: x => x.UserProductId,
                        principalTable: "UserProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProductsPaymentRecords_UserProductId",
                table: "UserProductsPaymentRecords",
                column: "UserProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProductsPaymentRecords");

            migrationBuilder.CreateTable(
                name: "UserProductsPaymentReccords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualAmountPaid = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ConfirmedAt = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DestinationAmount = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    DestinationCurrency = table.Column<string>(nullable: true),
                    ExpiresAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    SourceAmount = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    SourceCurrency = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UserProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProductsPaymentReccords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProductsPaymentReccords_UserProducts_UserProductId",
                        column: x => x.UserProductId,
                        principalTable: "UserProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProductsPaymentReccords_UserProductId",
                table: "UserProductsPaymentReccords",
                column: "UserProductId");
        }
    }
}
