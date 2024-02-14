using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingPathwaysInstitutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update Institution set IsActive = 0, IsAcceptingApplications = 0
                                    where InstitutionTypeID in (
	                                    select InstitutionTypeID from InstitutionType where name = 'Pathways CC'
                                    )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
