using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class CommimentStatusDisplayUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Display",
                table: "CommitmentStatus",
                newName: "StudentDisplay");

            migrationBuilder.AddColumn<string>(
                name: "AdminDisplay",
                table: "CommitmentStatus",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.Sql(@"update CommitmentStatus set AdminDisplay = StudentDisplay");

            migrationBuilder.Sql(@"update CommitmentStatus set AdminDisplay = 'PI Recommended Reject'
                where Value = 'PIReject'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminDisplay",
                table: "CommitmentStatus");

            migrationBuilder.RenameColumn(
                name: "StudentDisplay",
                table: "CommitmentStatus",
                newName: "Display");

            migrationBuilder.Sql(@"update CommitmentStatus set AdminDisplay = ''");

            migrationBuilder.Sql(@"update CommitmentStatus set AdminDisplay = 'Review Pending'
                where Value = 'PIReject'");
        }
    }
}
