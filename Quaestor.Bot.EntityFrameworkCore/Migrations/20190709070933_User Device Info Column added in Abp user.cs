using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class UserDeviceInfoColumnaddedinAbpuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceFCMToken",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceUUID",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceFCMToken",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DeviceUUID",
                table: "AbpUsers");
        }
    }
}
