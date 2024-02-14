using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingStudentInstituteFundingTableAddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DegreeMinorId",
                table: "StudentInstituteFunding",
                 type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondDegreeMajorID",
                table: "StudentInstituteFunding",
                 type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondDegreeMinorID",
                table: "StudentInstituteFunding",
                 type: "int",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_StudentInstituteFunding_MinorID",
                table: "StudentInstituteFunding",
                column: "DegreeMinorId");

            migrationBuilder.CreateIndex(
             name: "IX_StudentInstituteFunding_SecondDegreeMajorID",
             table: "StudentInstituteFunding",
             column: "SecondDegreeMajorID");

            migrationBuilder.CreateIndex(
             name: "IX_StudentInstituteFunding_SecondDegreeMinorID",
             table: "StudentInstituteFunding",
             column: "SecondDegreeMinorID");


            migrationBuilder.AddForeignKey(
            name: "FK_StudentInstituteFunding_DegreeMinor_MinorID",
            table: "StudentInstituteFunding",
            column: "DegreeMinorId",
            principalTable: "DegreeMinor",
            principalColumn: "DegreeMinorId");

            migrationBuilder.AddForeignKey(
            name: "FK_Student_StudentInstituteFunding_SecondDegreeMajorID",
            table: "StudentInstituteFunding",
            column: "SecondDegreeMajorID",
            principalTable: "DegreeMinor",
            principalColumn: "DegreeMinorId");

            migrationBuilder.AddForeignKey(
            name: "FK_Student_StudentInstituteFunding_SecondDegreeMinorID",
            table: "StudentInstituteFunding",
            column: "SecondDegreeMinorID",
            principalTable: "DegreeMinor",
            principalColumn: "DegreeMinorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstituteFunding_DegreeMinor_MinorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstituteFunding_DegreeMinor_SecondDegreeMajorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstituteFunding_DegreeMinor_SecondDegreeMinorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstituteFunding_MinorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstituteFunding_SecondDegreeMajorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstituteFunding_SecondDegreeMinorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropColumn(
                name: "DegreeMinorId",
                table: "StudentInstituteFunding");

            migrationBuilder.DropColumn(
                name: "-SecondDegreeMajorID",
                table: "StudentInstituteFunding");

            migrationBuilder.DropColumn(
                name: "SecondDegreeMinorID",
                table: "StudentInstituteFunding");
        }
    }
}
