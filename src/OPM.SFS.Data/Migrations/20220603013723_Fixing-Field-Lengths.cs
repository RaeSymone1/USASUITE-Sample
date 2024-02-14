using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class FixingFieldLengths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "AcademiaUser",
                type: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
               name: "FileName",
               table: "StudentDocument",
               type: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
              name: "Description",
              table: "StudentJobActivity",
              type: "varchar(max)");

            migrationBuilder.AlterColumn<string>(
             name: "StatusOther",
             table: "StudentJobActivity",
             type: "varchar(max)");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "Lastname",
              table: "AcademiaUser",
              type: "varchar(15)");

            migrationBuilder.AlterColumn<string>(
               name: "FileName",
               table: "StudentDocument",
               type: "varchar(100)");

            migrationBuilder.AlterColumn<string>(
              name: "Description",
              table: "StudentJobActivity",
              type: "varchar(8000)");

            migrationBuilder.AlterColumn<string>(
             name: "StatusOther",
             table: "StudentJobActivity",
             type: "varchar(8000)");
        }
    }
}
