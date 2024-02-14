using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixTESTAccount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"declare @dbserverName varchar(250)
            select @dbserverName = @@SERVERNAME
            if @dbserverName = 'sqlmi-sfs-test-e2-001.17ddc25c7056.database.windows.net'
            begin
	            update student set SSN = 'Fqt+rZk86kqZltkuwPTCjA=='
                where StudentID = 1303
            end";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
