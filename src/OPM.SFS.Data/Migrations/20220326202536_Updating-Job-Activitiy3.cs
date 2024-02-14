using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingJobActivitiy3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentJobActivity_Contact_ContactID",
                table: "StudentJobActivity");

            migrationBuilder.DropIndex(
                name: "IX_StudentJobActivity_ContactID",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "ContactID",
                table: "StudentJobActivity");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "StudentJobActivity",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactFirstName",
                table: "StudentJobActivity",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLastName",
                table: "StudentJobActivity",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "StudentJobActivity",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInserted",
                table: "StudentJobActivity",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "ContactFirstName",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "ContactLastName",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "StudentJobActivity");

            migrationBuilder.DropColumn(
                name: "DateInserted",
                table: "StudentJobActivity");

            migrationBuilder.AddColumn<int>(
                name: "ContactID",
                table: "StudentJobActivity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentJobActivity_ContactID",
                table: "StudentJobActivity",
                column: "ContactID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentJobActivity_Contact_ContactID",
                table: "StudentJobActivity",
                column: "ContactID",
                principalTable: "Contact",
                principalColumn: "ContactID");
        }
    }
}
