using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class FixingPITable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AcademiaUser",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "AcademiaUser",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "AcademiaUser",
                type: "varchar(15)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "AcademiaUser",
                type: "varchar(15)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AcademiaUser",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExtension",
                table: "AcademiaUser",
                type: "varchar(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "AcademiaUser");
        }
    }
}
