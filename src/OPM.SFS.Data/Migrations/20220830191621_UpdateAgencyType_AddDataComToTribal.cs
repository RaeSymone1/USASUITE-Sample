using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdateAgencyType_AddDataComToTribal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                UPDATE AgencyType
                SET [ValidEmailDomain] = '[''.gov'', ''.mil'', ''.edu'', ''.museum'', ''.org'', ''.us'', ''.com'']'
                WHERE Name = 'Tribal' AND code = 'Tribal'

            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                UPDATE AgencyType
                SET [ValidEmailDomain] = '[''.gov'', ''.mil'', ''.edu'', ''.museum'', ''.org'', ''.us'']'
                WHERE Name = 'Tribal' AND code = 'Tribal'

            ");
        }
    }
}
