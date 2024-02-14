using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AgencyCommitmentWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Display",
                table: "CommitmentApprovalWorkflow",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.Sql(@"update CommitmentApprovalWorkflow set Display = 'PreApproval Required' where code = 'Tentative';
                                   update CommitmentApprovalWorkflow set Display = 'Approved/Full Form' where code = 'Final'");

            migrationBuilder.Sql(@"declare @wkID int
                                    Select @wkID = commitmentapprovalworkflowid from CommitmentApprovalWorkflow
                                    where code = 'Tentative';
                                    Update agency set CommitmentApprovalWorkflowId = @wkID
                                    where CommitmentApprovalWorkflowId is null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                table: "CommitmentApprovalWorkflow");
        }
    }
}
