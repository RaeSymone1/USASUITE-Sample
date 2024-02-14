using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingStudentFMLCharacterLimitFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "FirstName",
              table: "Student",
              type: "varchar(255)");
            migrationBuilder.AlterColumn<string>(
              name: "MiddleName",
              table: "Student",
              type: "varchar(50)",
              nullable: true);
            migrationBuilder.AlterColumn<string>(
              name: "LastName",
              table: "Student",
              type: "varchar(50)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
