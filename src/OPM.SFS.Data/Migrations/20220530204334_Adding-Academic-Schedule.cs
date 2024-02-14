using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingAcademicSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicScheduleId",
                table: "Institution",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptingApplications",
                table: "Institution",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AcademicSchedule",
                columns: table => new
                {
                    AcademicScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSchedule", x => x.AcademicScheduleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Institution_AcademicScheduleId",
                table: "Institution",
                column: "AcademicScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_AcademicSchedule_AcademicScheduleId",
                table: "Institution",
                column: "AcademicScheduleId",
                principalTable: "AcademicSchedule",
                principalColumn: "AcademicScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_AcademicSchedule_AcademicScheduleId",
                table: "Institution");

            migrationBuilder.DropTable(
                name: "AcademicSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Institution_AcademicScheduleId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "AcademicScheduleId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "IsAcceptingApplications",
                table: "Institution");
        }
    }
}
