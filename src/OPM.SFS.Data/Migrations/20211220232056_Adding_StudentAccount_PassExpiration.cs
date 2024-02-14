using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPM.SFS.Data.Migrations
{
    public partial class Adding_StudentAccount_PassExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordExpiration",
                table: "Student");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpiration",
                table: "StudentAccount",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccount_StudentID",
                table: "StudentAccount",
                column: "StudentID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccount_Student_StudentID",
                table: "StudentAccount",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccount_Student_StudentID",
                table: "StudentAccount");

            migrationBuilder.DropIndex(
                name: "IX_StudentAccount_StudentID",
                table: "StudentAccount");

            migrationBuilder.DropColumn(
                name: "PasswordExpiration",
                table: "StudentAccount");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordExpiration",
                table: "Student",
                type: "date",
                nullable: true);
        }
    }
}
