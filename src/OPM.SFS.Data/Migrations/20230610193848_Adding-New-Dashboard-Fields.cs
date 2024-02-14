using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewDashboardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePendingReleaseCollectionInfo",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReleasedCollectionPackage",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePendingReleaseCollectionInfo",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "DateReleasedCollectionPackage",
                table: "StudentInstitutionFundings");
        }
    }
}
