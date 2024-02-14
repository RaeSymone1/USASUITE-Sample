using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class TableCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
IF (OBJECT_ID('dbo.FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS', 'F') IS NOT NULL)
BEGIN
    ALTER TABLE dbo.QRTZ_TRIGGERS DROP CONSTRAINT FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_BLOB_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_BLOB_TRIGGERS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_CALENDARS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_CALENDARS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_CRON_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_CRON_TRIGGERS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_FIRED_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_FIRED_TRIGGERS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_JOB_DETAILS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_JOB_DETAILS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_LOCKS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_LOCKS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_PAUSED_TRIGGER_GRPS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_PAUSED_TRIGGER_GRPS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_SCHEDULER_STATE]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_SCHEDULER_STATE]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_SIMPLE_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_SIMPLE_TRIGGERS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_SIMPROP_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_SIMPROP_TRIGGERS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QRTZ_TRIGGERS]') AND type in (N'U'))
DROP TABLE [dbo].[QRTZ_TRIGGERS]
GO

";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
