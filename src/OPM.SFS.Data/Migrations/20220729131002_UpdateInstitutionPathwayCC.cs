using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdateInstitutionPathwayCC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"declare @institeTypeId int

            select @institeTypeId = institutiontypeid from InstitutionType where name = 'Pathways CC'

            update institution
            set IsActive = 0, IsAcceptingApplications = 0, InstitutionTypeID = @institeTypeId
            where name like '%/%'
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
