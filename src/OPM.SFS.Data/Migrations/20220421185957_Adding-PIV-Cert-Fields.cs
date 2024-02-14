using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingPIVCertFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Certificate",
                table: "AgencyUser",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "AgencyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "AgencyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "AgencyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectAlternative",
                table: "AgencyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbprint",
                table: "AgencyUser",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidAfter",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Certificate",
                table: "AdminUser",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectAlternative",
                table: "AdminUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbprint",
                table: "AdminUser",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidAfter",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificateStaging",
                columns: table => new
                {
                    CertificateStagingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Thumbprint = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectAlternative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidAfter = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Certificate = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateInserted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateStaging", x => x.CertificateStagingID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateStaging");

            migrationBuilder.DropColumn(
                name: "Certificate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "SubjectAlternative",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "Thumbprint",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "ValidAfter",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "Certificate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "SubjectAlternative",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Thumbprint",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "ValidAfter",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "AdminUser");
        }
    }
}
