using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quaestor.Bot.Migrations
{
    public partial class changeuserproducttofullEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "UserProducts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "UserProducts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserProducts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "UserProducts",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "UserProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "UserProducts");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "UserProducts");
        }
    }
}
