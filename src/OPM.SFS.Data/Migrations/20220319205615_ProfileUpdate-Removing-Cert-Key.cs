using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class ProfileUpdateRemovingCertKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentSecurityCertificationId",
                table: "StudentSecurityCertification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentSecurityCertificationId",
                table: "StudentSecurityCertification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
