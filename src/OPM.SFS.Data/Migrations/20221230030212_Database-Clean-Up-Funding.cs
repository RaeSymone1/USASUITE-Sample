using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCleanUpFunding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentInstituteFunding");

            migrationBuilder.Sql("DROP TABLE IF EXISTS DegreeMinor");

            migrationBuilder.DropColumn(
                name: "ExpectedGradDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "InternshipAvailDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PostGradAvailDate",
                table: "Student");

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedGradDate",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InternshipAvailDate",
                table: "Student",
                type: "datetime2",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostGradAvailDate",
                table: "Student",
                type: "datetime2",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DegreeMinor",
                columns: table => new
                {
                    DegreeMinorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeMinor", x => x.DegreeMinorId);
                });

            migrationBuilder.CreateTable(
                name: "StudentInstituteFunding",
                columns: table => new
                {
                    StudentInstituteFundingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeId = table.Column<int>(type: "int", nullable: true),
                    DegreeMinorId = table.Column<int>(type: "int", nullable: true),
                    DisciplineId = table.Column<int>(type: "int", nullable: true),
                    InstitutionId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    EnrolledSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrolledYear = table.Column<int>(type: "int", nullable: true),
                    ExpectedGradDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FundingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FundingEndSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FundingEndYear = table.Column<int>(type: "int", nullable: true),
                    InitialFundingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondDegreeMajorId = table.Column<int>(type: "int", nullable: true),
                    SecondDegreeMinorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInstituteFunding", x => x.StudentInstituteFundingId);
                    table.ForeignKey(
                        name: "FK_StudentInstituteFunding_DegreeMinor_DegreeMinorId",
                        column: x => x.DegreeMinorId,
                        principalTable: "DegreeMinor",
                        principalColumn: "DegreeMinorId");
                    table.ForeignKey(
                        name: "FK_StudentInstituteFunding_Degree_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "Degree",
                        principalColumn: "DegreeID");
                    table.ForeignKey(
                        name: "FK_StudentInstituteFunding_Discipline_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Discipline",
                        principalColumn: "DisciplineID");
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
                name: "IX_StudentInstituteFunding_DegreeId",
                table: "StudentInstituteFunding",
                column: "DegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_DegreeMinorId",
                table: "StudentInstituteFunding",
                column: "DegreeMinorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_DisciplineId",
                table: "StudentInstituteFunding",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_InstitutionId",
                table: "StudentInstituteFunding",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_StudentId",
                table: "StudentInstituteFunding",
                column: "StudentId");
        }
    }
}
