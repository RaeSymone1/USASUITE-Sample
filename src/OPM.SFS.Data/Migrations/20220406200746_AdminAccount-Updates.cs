using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AdminAccountUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnforcePIV",
                table: "AdminUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AdminUser",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordReset",
                table: "AdminUser",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AdminUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AdminUser",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PIVOverrideExpiration",
                table: "AdminUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PasswordCrypto",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ROBExpiration",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnforcePIV",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "ForcePasswordReset",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "PIVOverrideExpiration",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "PasswordCrypto",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "ROBExpiration",
                table: "AdminUser");
        }
    }
}
