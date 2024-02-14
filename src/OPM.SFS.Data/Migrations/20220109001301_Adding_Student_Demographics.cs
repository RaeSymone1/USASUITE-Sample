using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class Adding_Student_Demographics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HighSchoolStateStateId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsMilitary",
                table: "Student",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolTypeID",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsInSecurityPosition",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolType",
                columns: table => new
                {
                    SchoolTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolType", x => x.SchoolTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_HighSchoolStateStateId",
                table: "Student",
                column: "HighSchoolStateStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_SchoolTypeID",
                table: "Student",
                column: "SchoolTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_SchoolType_SchoolTypeID",
                table: "Student",
                column: "SchoolTypeID",
                principalTable: "SchoolType",
                principalColumn: "SchoolTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_State_HighSchoolStateStateId",
                table: "Student",
                column: "HighSchoolStateStateId",
                principalTable: "State",
                principalColumn: "StateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_SchoolType_SchoolTypeID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_State_HighSchoolStateStateId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "SchoolType");

            migrationBuilder.DropIndex(
                name: "IX_Student_HighSchoolStateStateId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_SchoolTypeID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "HighSchoolStateStateId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IsMilitary",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "SchoolTypeID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "YearsInSecurityPosition",
                table: "Student");
        }
    }
}
