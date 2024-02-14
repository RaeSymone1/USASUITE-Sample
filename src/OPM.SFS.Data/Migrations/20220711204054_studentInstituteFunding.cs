using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class studentInstituteFunding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentInstituteFunding",
                columns: table => new
                {
                    StudentInstituteFundingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InstitutionId = table.Column<int>(type: "int", nullable: true),
                    DisciplineId = table.Column<int>(type: "int", nullable: true),
                    DegreeId = table.Column<int>(type: "int", nullable: true),
                    ExpectedGradDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnrolledSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrolledYear = table.Column<int>(type: "int", nullable: true),
                    FundingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InitialFundingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FundingEndYear = table.Column<int>(type: "int", nullable: true),
                    FundingEndSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInstituteFunding", x => x.StudentInstituteFundingId);
                    table.ForeignKey(
                        name: "FK_StudentInstituteFunding_Institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institution",
                        principalColumn: "InstitutionID");
                    table.ForeignKey(
                        name: "FK_StudentInstituteFunding_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_InstitutionId",
                table: "StudentInstituteFunding",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_StudentId",
                table: "StudentInstituteFunding",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentInstituteFunding");
        }
    }
}
