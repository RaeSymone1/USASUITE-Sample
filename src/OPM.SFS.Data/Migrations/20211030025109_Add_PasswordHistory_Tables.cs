using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OPM.SFS.Data.Migrations
{
    public partial class Add_PasswordHistory_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademiaUserPasswordHistory",
                columns: table => new
                {
                    AcademiaUserPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademiaUserID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademiaUserPasswordHistory", x => x.AcademiaUserPasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_AcademiaUserPasswordHistory_AcademiaUser_AcademiaUserID",
                        column: x => x.AcademiaUserID,
                        principalTable: "AcademiaUser",
                        principalColumn: "AcademiaUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserPasswordHistory",
                columns: table => new
                {
                    AdminUserPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUserID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserPasswordHistory", x => x.AdminUserPasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_AdminUserPasswordHistory_AdminUser_AdminUserID",
                        column: x => x.AdminUserID,
                        principalTable: "AdminUser",
                        principalColumn: "AdminUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgencyUserPasswordHistory",
                columns: table => new
                {
                    AgencyUserPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgencyUserID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyUserPasswordHistory", x => x.AgencyUserPasswordHistoryID);
                    table.ForeignKey(
                        name: "FK_AgencyUserPasswordHistory_AgencyUser_AgencyUserID",
                        column: x => x.AgencyUserID,
                        principalTable: "AgencyUser",
                        principalColumn: "AgencyUserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPasswordHistory",
                columns: table => new
                {
                    StudentPasswordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "varchar(256)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_AcademiaUserPasswordHistory_AcademiaUserID",
                table: "AcademiaUserPasswordHistory",
                column: "AcademiaUserID");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUserPasswordHistory_AdminUserID",
                table: "AdminUserPasswordHistory",
                column: "AdminUserID");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyUserPasswordHistory_AgencyUserID",
                table: "AgencyUserPasswordHistory",
                column: "AgencyUserID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPasswordHistory_StudentID",
                table: "StudentPasswordHistory",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademiaUserPasswordHistory");

            migrationBuilder.DropTable(
                name: "AdminUserPasswordHistory");

            migrationBuilder.DropTable(
                name: "AgencyUserPasswordHistory");

            migrationBuilder.DropTable(
                name: "StudentPasswordHistory");
        }
    }
}
