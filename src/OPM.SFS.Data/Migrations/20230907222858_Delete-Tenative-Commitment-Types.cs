using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTenativeCommitmentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"delete from CommitmentType where name in ('Tentative Internship', 'Tentative Postgraduate')";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
