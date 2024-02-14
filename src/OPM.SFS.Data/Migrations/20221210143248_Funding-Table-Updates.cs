using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FundingTableUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
              name: "InternshipAvailDate",
              table: "StudentInstitutionFundings",
              type: "datetime2",
              nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostGradAvailDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "InternshipAvailDate",
               table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "PostGradAvailDate",
                table: "StudentInstitutionFundings");
        }
    }
}
