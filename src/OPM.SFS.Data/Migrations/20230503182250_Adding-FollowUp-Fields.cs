using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingFollowUpFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FollowUpAction",
                table: "StudentInstitutionFundings",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FollowUpTypeOptionID",
                table: "StudentInstitutionFundings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstitutionFundings_FollowUpTypeOptionID",
                table: "StudentInstitutionFundings",
                column: "FollowUpTypeOptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInstitutionFundings_FollowUpTypeOption_FollowUpTypeOptionID",
                table: "StudentInstitutionFundings",
                column: "FollowUpTypeOptionID",
                principalTable: "FollowUpTypeOption",
                principalColumn: "FollowUpTypeOptionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInstitutionFundings_FollowUpTypeOption_FollowUpTypeOptionID",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropIndex(
                name: "IX_StudentInstitutionFundings_FollowUpTypeOptionID",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "FollowUpAction",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "FollowUpTypeOptionID",
                table: "StudentInstitutionFundings");
        }
    }
}
