using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdateProfileStatusAddColumnDisplay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ProfileStatus ADD Display Varchar(255);
                go

                UPDATE ProfileStatus
                SET [Display] = 'Account is in good standing and can be logged into.'
                WHERE Name = 'Active'

                UPDATE ProfileStatus
                SET [Display] = 'Account has been disabled due to inactivity; user has not logged in for the past 365 days'
                WHERE Name = 'Inactive'

                UPDATE ProfileStatus
                SET [Display] = 'New account has been created and needs admin approval.'
                WHERE Name = 'Pending'

                UPDATE ProfileStatus
                SET [Display] = 'Admin manually disabled account. Not accessible to users to login.'
                WHERE Name = 'Disabled'

                UPDATE ProfileStatus
                SET [Display] = 'User has a temporary password that will expire within 24 hours of generation. By either admins or user password resets.'
                WHERE Name = 'Temporary'


            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
