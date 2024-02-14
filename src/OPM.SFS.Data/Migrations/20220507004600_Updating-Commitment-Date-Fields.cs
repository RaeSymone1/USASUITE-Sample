using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdatingCommitmentDateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PIStatus",
                table: "StudentCommitment",
                newName: "PIRecommendation");

            migrationBuilder.RenameColumn(
                name: "LastApprovedByPI",
                table: "StudentCommitment",
                newName: "LastUpdatedByPIID");

            migrationBuilder.RenameColumn(
                name: "LastApprovedByAdmin",
                table: "StudentCommitment",
                newName: "LastUpdatedByAdminID");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "StudentCommitment",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "StudentCommitment");

            migrationBuilder.RenameColumn(
                name: "PIRecommendation",
                table: "StudentCommitment",
                newName: "PIStatus");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedByPIID",
                table: "StudentCommitment",
                newName: "LastApprovedByPI");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedByAdminID",
                table: "StudentCommitment",
                newName: "LastApprovedByAdmin");
        }
    }
}
