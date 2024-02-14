using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class CommimentStatusCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "CommitmentStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"update CommitmentStatus set IsDisabled = 1
                    Where Value in ('PIRequestInfo', 'PORequestInfo', 'PORequestFOL', 'PIApprovalPendingFOL', 'PIRequestFOLInfo', 'PORejectFOL'
                    ,'PORequestPosDesc', 'POApprovalPendingPosDesc','PIRejectFOL', 'PORequestFOLInfo', 'POApprovalPendingFOL')");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'PI Review Pending'
                where Value = 'PIApprovalPending'");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'PO Review Pending'
                where Value = 'POApprovalPending'");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'Final Review Pending'
                where Value = 'POApprovalPendingFOLPosDesc'");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "CommitmentStatus");

            migrationBuilder.Sql(@"update CommitmentStatus set IsDisabled = 0");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'Review Pending'
                where Value = 'PIApprovalPending'");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'Review Pending'
                where Value = 'POApprovalPending'");

            migrationBuilder.Sql(@"update CommitmentStatus set Display = 'Review Pending'
                where Value = 'POApprovalPendingFOLPosDesc'");

        }
    }
}
