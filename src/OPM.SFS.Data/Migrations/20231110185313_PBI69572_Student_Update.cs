using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69572StudentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"update funding
set notes = '', followupaction = ''
from StudentInstitutionFundings funding
inner join Student s on funding.StudentId = s.StudentID
where s.StudentUID = 11930";
            migrationBuilder.Sql(script);   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
