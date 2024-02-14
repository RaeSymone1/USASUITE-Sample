using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('*/3 * * * *', 'COMPLETE', NULL, GETUTCDATE(), 'SendEmailTask', 0);
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('*/3 * * * *', 'COMPLETE', NULL, GETUTCDATE(), 'CheckMalwareResultsTask', 0);
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('0 3 * * *', 'COMPLETE', NULL, GETUTCDATE(), 'StudentRegistrationReminderTask', 0);
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('0 4 * * *', 'COMPLETE', NULL, GETUTCDATE(), 'PostgradVerificationDueReminderTask', 0);
insert into ScheduledTask (Schedule, State, LastRunDate, LastUpdated, Name, IsDisabled)
values ('0 5 * * *', 'COMPLETE', NULL, GETUTCDATE(), 'ServiceObligationCompleteReminderTask', 0);
";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
