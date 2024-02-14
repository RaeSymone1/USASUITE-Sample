using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingPGVerificationDueDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DateLeftPGEarly",
            //    table: "StudentInstitutionFundings",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PGVerificationOneCompleteDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PGVerificationOneDueDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PGVerificationTwoCompleteDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PGVerificationTwoDueDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Extension",
            //    table: "ExtensionType",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "DateLeftPGEarly",
            //    table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PGVerificationOneCompleteDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PGVerificationOneDueDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PGVerificationTwoCompleteDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PGVerificationTwoDueDate",
                table: "StudentInstitutionFundings");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Extension",
            //    table: "ExtensionType",
            //    type: "nvarchar(50)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);
        }
    }
}
