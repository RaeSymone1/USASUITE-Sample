using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingForcePasswordResetfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordReset",
                table: "AgencyUser",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordReset",
                table: "AcademiaUser",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcePasswordReset",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "ForcePasswordReset",
                table: "AcademiaUser");
        }
    }
}
