using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingProfileStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileStatusID",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileStatusID",
                table: "AgencyUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileStatusID",
                table: "AcademiaUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProfileStatus",
                columns: table => new
                {
                    ProfileStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStatus", x => x.ProfileStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_ProfileStatusID",
                table: "Student",
                column: "ProfileStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_AcademiaUser_ProfileStatusID",
                table: "AcademiaUser",
                column: "ProfileStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademiaUser_ProfileStatus_ProfileStatusID",
                table: "AcademiaUser",
                column: "ProfileStatusID",
                principalTable: "ProfileStatus",
                principalColumn: "ProfileStatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_ProfileStatus_ProfileStatusID",
                table: "Student",
                column: "ProfileStatusID",
                principalTable: "ProfileStatus",
                principalColumn: "ProfileStatusID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademiaUser_ProfileStatus_ProfileStatusID",
                table: "AcademiaUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_ProfileStatus_ProfileStatusID",
                table: "Student");

            migrationBuilder.DropTable(
                name: "ProfileStatus");

            migrationBuilder.DropIndex(
                name: "IX_Student_ProfileStatusID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_AcademiaUser_ProfileStatusID",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "ProfileStatusID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ProfileStatusID",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "ProfileStatusID",
                table: "AcademiaUser");
        }
    }
}
