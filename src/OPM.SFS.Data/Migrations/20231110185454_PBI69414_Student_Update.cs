using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class PBI69414StudentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = @"--Christopher Colvin
UPDATE StudentInstitutionFundings SET StudentId = 8723 WHERE StudentInstitutionFundingId = 6240
--Disable the duplicate student acount
update student set ProfileStatusID = 27
where StudentUID = 14038

--Rahmira Rufus
UPDATE StudentInstitutionFundings SET StudentId = 4879 WHERE StudentInstitutionFundingId = 2289
--Disable the duplicate student acount
update student set ProfileStatusID = 27
where StudentUID = 4603

--Matthew Jackson
UPDATE StudentInstitutionFundings SET StudentId = 6476 WHERE StudentInstitutionFundingId = 2578
--Disable the duplicate student acount
update student set ProfileStatusID = 27
where StudentUID = 6091";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
