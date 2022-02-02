using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class Added_UserSessionDetail_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSessionDetail",
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
                    UserSessionId = table.Column<int>(nullable: false),
                    MarketId = table.Column<int>(nullable: false),
                    BTCAllocated = table.Column<decimal>(nullable: false),
                    TradeProfitPercentage = table.Column<decimal?>(nullable: true),
                    FirstBuyEquityPercentage = table.Column<decimal?>(nullable: true),
                    RebuyPercentage = table.Column<decimal?>(nullable: true),
                    BuyStopUp = table.Column<decimal?>(nullable: true),
                    BuyStopDown = table.Column<decimal?>(nullable: true),
                    FirstRebuy = table.Column<decimal?>(nullable: true),
                    SecondRebuy = table.Column<decimal?>(nullable: true),
                    ThirdRebuy = table.Column<decimal?>(nullable: true),
                    FourthRebuy = table.Column<decimal?>(nullable: true),
                    FifthRebuy = table.Column<decimal?>(nullable: true),
                    SixthRebuy = table.Column<decimal?>(nullable: true),
                    SeventRebuy = table.Column<decimal?>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessionDetail_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSessionDetail_UserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalTable: "UserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionDetail_MarketId",
                table: "UserSessionDetail",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessionDetail_UserSessionId",
                table: "UserSessionDetail",
                column: "UserSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSessionDetail");
        }
    }
}
