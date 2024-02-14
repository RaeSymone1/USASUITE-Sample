using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class DBCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowupType");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Student");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CommitmentPhaseComplete",
            //    table: "StudentInstitutionFundings",
            //    type: "datetime2",
            //    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitmentPhaseComplete",
                table: "StudentInstitutionFundings");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FollowupType",
                columns: table => new
                {
                    FollowupTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowupType", x => x.FollowupTypeId);
                });
        }
    }
}
