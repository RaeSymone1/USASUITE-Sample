using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class ForgotPasswordUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordCrypto",
                table: "StudentAccountPasswordHistory",
                type: "varchar(25)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForcePasswordReset",
                table: "StudentAccount",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordCrypto",
                table: "StudentAccountPasswordHistory");

            migrationBuilder.DropColumn(
                name: "ForcePasswordReset",
                table: "StudentAccount");
        }
    }
}
