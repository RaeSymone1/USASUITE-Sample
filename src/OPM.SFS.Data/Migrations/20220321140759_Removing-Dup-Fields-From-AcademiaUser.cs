using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class RemovingDupFieldsFromAcademiaUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fax",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AcademiaUser");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "AcademiaUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "AcademiaUser",
                type: "varchar(30)",
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
    }
}
