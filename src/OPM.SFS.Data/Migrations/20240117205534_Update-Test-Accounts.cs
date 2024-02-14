using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmploymentVerification_StudentCommitmentId",
                table: "EmploymentVerification",
                column: "StudentCommitmentId",
                unique: true,
                filter: "[StudentCommitmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentVerification_StudentCommitment_StudentCommitmentId",
                table: "EmploymentVerification",
                column: "StudentCommitmentId",
                principalTable: "StudentCommitment",
                principalColumn: "StudentCommitmentID");

            var script = @"declare @dbserverName varchar(250)
            select @dbserverName = @@SERVERNAME
            if @dbserverName = 'sqlmi-sfs-test-e2-001.17ddc25c7056.database.windows.net'
            begin
	            update AcademiaUser set ProfileStatusID = 2
	            where email like '$invalid%'
            end";
            migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentVerification_StudentCommitment_StudentCommitmentId",
                table: "EmploymentVerification");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentVerification_StudentCommitmentId",
                table: "EmploymentVerification");
        }
    }
}
