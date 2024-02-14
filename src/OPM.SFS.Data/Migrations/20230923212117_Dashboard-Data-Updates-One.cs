using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DashboardDataUpdatesOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var script = @"
--updating PR citizenship
update student set CitizenshipID = 2
where StudentUID in (7878, 7797, 9175, 13913,11626,13396,11945,11365,13550,7905,11704,11743,7420,13197,9142,11946,11651,11179,11051,13626,9146,7931)
and CitizenshipID is null;

--setting all others to US
update Student set CitizenshipID = 1
where CitizenshipID is null;

--Removing bad data from InternshipAgencyType.
--The commitment reload will fix this field
update StudentInstitutionFundings 
set InternshipAgencyType = null
where internshipagencytype in ('Yes', 'No');           
                
--Fixing Citizenship spelling issue
update Citizenship set value = 'Permanent Resident'
where value = 'Permanent Residance'
";
			migrationBuilder.Sql(script);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
