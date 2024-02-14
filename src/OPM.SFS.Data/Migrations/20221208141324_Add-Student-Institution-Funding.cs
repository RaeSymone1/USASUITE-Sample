using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddStudentInstitutionFunding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentInstitutionFundings",
                columns: table => new
                {
                    StudentInstitutionFundingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InstitutionId = table.Column<int>(type: "int", nullable: true),
                    MajorId = table.Column<int>(type: "int", nullable: true),
                    DegreeId = table.Column<int>(type: "int", nullable: true),
                    MinorId = table.Column<int>(type: "int", nullable: true),
                    SecondDegreeMajorId = table.Column<int>(type: "int", nullable: true),
                    SecondDegreeMinorId = table.Column<int>(type: "int", nullable: true),
                    ExpectedGradDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnrolledSession = table.Column<string>(type: "varchar(50)", nullable: true),
                    EnrolledYear = table.Column<int>(type: "int", nullable: true),
                    FundingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InitialFundingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FundingEndYear = table.Column<int>(type: "int", nullable: true),
                    FundingEndSession = table.Column<string>(type: "varchar(50)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInstitutionFundings", x => x.StudentInstitutionFundingId);
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Degree_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degree",
                        principalColumn: "DegreeID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Discipline_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Discipline_MinorId",
                        column: x => x.MinorId,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Discipline_SecondDegreeMajorId",
                        column: x => x.SecondDegreeMajorId,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Discipline_SecondDegreeMinorId",
                        column: x => x.SecondDegreeMinorId,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institution",
                        principalColumn: "InstitutionID");
                    table.ForeignKey(
                        name: "FK_StudentInstitutionFundings_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_DegreeId",
                table: "StudentInstitutionFundings",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_InstitutionId",
                table: "StudentInstitutionFundings",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_MajorId",
                table: "StudentInstitutionFundings",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_MinorId",
                table: "StudentInstitutionFundings",
                column: "MinorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_SecondDegreeMajorId",
                table: "StudentInstitutionFundings",
                column: "SecondDegreeMajorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_SecondDegreeMinorId",
                table: "StudentInstitutionFundings",
                column: "SecondDegreeMinorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_StudentId",
                table: "StudentInstitutionFundings",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentInstitutionFundings");
        }
    }
}
