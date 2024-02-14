using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPM.SFS.Data.Migrations
{
    public partial class Adding_StudentAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentPasswordHistory");

            migrationBuilder.DropColumn(
                name: "FailedLoginCount",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "FailedLoginDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Student");

            migrationBuilder.CreateTable(
                name: "StudentAccount",
                columns: table => new
                {
                    StudentAccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", nullable: true),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    FailedLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAccount", x => x.StudentAccountID);
                });

            migrationBuilder.CreateTable(
                name: "StudentAccountPasswordHistory",
                columns: table => new
                {
                    StudentAccountPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentAccountID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAccountPasswordHistory", x => x.StudentAccountPasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_StudentAccountPasswordHistory_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentAccountPasswordHistory_StudentAccount_StudentAccountID",
                        column: x => x.StudentAccountID,
                        principalTable: "StudentAccount",
                        principalColumn: "StudentAccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccountPasswordHistory_StudentAccountID",
                table: "StudentAccountPasswordHistory",
                column: "StudentAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccountPasswordHistory_StudentId",
                table: "StudentAccountPasswordHistory",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAccountPasswordHistory");

            migrationBuilder.DropTable(
                name: "StudentAccount");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginCount",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedLoginDate",
                table: "Student",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Student",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Student",
                type: "varchar(256)",
                unicode: false,
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Student",
                type: "varchar(120)",
                unicode: false,
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StudentPasswordHistory",
                columns: table => new
                {
                    StudentPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPasswordHistory", x => x.StudentPasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_StudentPasswordHistory_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPasswordHistory_StudentID",
                table: "StudentPasswordHistory",
                column: "StudentID");
        }
    }
}
