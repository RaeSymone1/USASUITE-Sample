using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FeatureTogglesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "Feature",
                newName: "IsEnabledSiteWide");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "StudentFeature",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AgencyUserFeature",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AdminFeature",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AcademiaUserFeature",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql($@"                
                INSERT INTO Feature(Name,IsEnabledSiteWide,LastModified)
                VALUES('UnregisteredStudentFlow',0,'{DateTime.Now}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "StudentFeature");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AgencyUserFeature");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AdminFeature");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AcademiaUserFeature");

            migrationBuilder.RenameColumn(
                name: "IsEnabledSiteWide",
                table: "Feature",
                newName: "IsEnabled");
        }
    }
}
