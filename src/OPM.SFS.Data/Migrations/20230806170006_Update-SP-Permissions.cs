using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSPPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"GRANT EXEC ON ScholarshipforService.dbo.GetAllStudentsForDashboard TO PUBLIC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
