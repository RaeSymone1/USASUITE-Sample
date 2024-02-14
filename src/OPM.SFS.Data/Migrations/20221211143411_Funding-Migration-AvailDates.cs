using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FundingMigrationAvailDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Fund
                SET Fund.InternshipAvailDate = S.InternshipAvailDate, fund.PostGradAvailDate = s.PostGradAvailDate
                FROM StudentInstitutionFundings Fund
                INNER JOIN Student S
                ON  Fund.StudentId  = S.StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Update StudentInstitutionFundings Set InternshipAvailDate = NULL, PostGradAvailDate = NULL");
        }
    }
}
