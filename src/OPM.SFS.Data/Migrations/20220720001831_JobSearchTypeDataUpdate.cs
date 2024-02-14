using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class JobSearchTypeDataUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete from JobSearchTypes where name = 'N/A' and code = 'N/A'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
