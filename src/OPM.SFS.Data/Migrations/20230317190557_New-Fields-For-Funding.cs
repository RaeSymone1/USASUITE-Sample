using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class NewFieldsForFunding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInternshipAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInternshipAgencyType",
                table: "StudentInstitutionFundings",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInternshipReportedWebsite",
                table: "StudentInstitutionFundings",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInternshipSubAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPostGradAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPostGradAgencyType",
                table: "StudentInstitutionFundings",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPostGradReportedWebsite",
                table: "StudentInstitutionFundings",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPostGradSubAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipAgencyType",
                table: "StudentInstitutionFundings",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InternshipEOD",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipSubAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipReported",
                table: "StudentInstitutionFundings",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostGradAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostGradAgencyType",
                table: "StudentInstitutionFundings",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostGradEOD",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostGradReported",
                table: "StudentInstitutionFundings",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostGradSubAgencyName",
                table: "StudentInstitutionFundings",
                type: "varchar(512)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ServiceOwed",
                table: "StudentInstitutionFundings",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInternshipAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalInternshipAgencyType",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalInternshipReportedWebsite",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalInternshipSubAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalPostGradAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalPostGradAgencyType",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalPostGradReportedWebsite",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "AdditionalPostGradSubAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "InternshipAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "InternshipAgencyType",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "InternshipEOD",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "InternshipSubAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "IntershipReported",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradAgencyType",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradEOD",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradReported",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradSubAgencyName",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "ServiceOwed",
                table: "StudentInstitutionFundings");
        }
    }
}
