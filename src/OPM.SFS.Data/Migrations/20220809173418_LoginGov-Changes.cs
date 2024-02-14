using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class LoginGovChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginGovLinkID",
                table: "Student",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginGovLinkID",
                table: "AgencyUser",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginGovLinkID",
                table: "AdminUser",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginGovLinkID",
                table: "AcademiaUser",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginGovStaging",
                columns: table => new
                {
                    LoginGovStagingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    AccountType = table.Column<string>(type: "varchar(50)", nullable: false),
                    LoginGovLinkID = table.Column<string>(type: "varchar(150)", nullable: false),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginGovStaging", x => x.LoginGovStagingID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginGovStaging");

            migrationBuilder.DropColumn(
                name: "LoginGovLinkID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LoginGovLinkID",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "LoginGovLinkID",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "LoginGovLinkID",
                table: "AcademiaUser");
        }
    }
}
