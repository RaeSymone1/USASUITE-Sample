using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('0 0 * * *', 'COMPLETE', NULL, GETDATE(), 'InactiveAccountReminderTask', 0);
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('0 2 * * *', 'COMPLETE', NULL, GETDATE(), 'SetAccountInactiveTask', 0);";
            migrationBuilder.Sql(script);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
