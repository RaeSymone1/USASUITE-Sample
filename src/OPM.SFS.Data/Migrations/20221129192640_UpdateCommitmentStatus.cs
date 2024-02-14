using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdateCommitmentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                UPDATE CommitmentStatus
                SET [Description] = 'PO is requesting final job offer letter and position description document'
                WHERE Value in ('PORequestPosDesc','PORequestFOLPosDesc')

                UPDATE CommitmentStatus
                SET [Description] = 'Position description document awaiting PO approval'
                WHERE Value = 'POApprovalPendingFOLPosDesc'

            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
