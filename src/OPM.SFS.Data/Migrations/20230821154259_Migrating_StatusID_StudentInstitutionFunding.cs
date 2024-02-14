using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigratingStatusIDStudentInstitutionFunding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
              name: "StatusID",
              table: "StudentInstitutionFundings",
              type: "int",
              nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Fund
                SET Fund.StatusID = S.StatusID
                FROM StudentInstitutionFundings Fund
                INNER JOIN Student S
                ON  Fund.StudentId  = S.StudentID");
            migrationBuilder.DropIndex(
             name: "IX_Student_StatusID",
             table: "Student");

            migrationBuilder.DropForeignKey(
             name: "FK_Student_StudentStatusOptions",
             table: "Student");

            migrationBuilder.DropColumn(
             name: "StatusID",
             table: "Student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
