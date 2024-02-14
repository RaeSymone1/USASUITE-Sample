using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class UpdateStudentCommitment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "StudentCommitment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentCommitment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "StudentCommitment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentCommitment");
        }
    }
}
