using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingReminderDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PostGradVerificationReminderSentDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceObligationCompleteReminderSentDate",
                table: "StudentInstitutionFundings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompleteRegistrationReminderSentDate",
                table: "Student",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostGradVerificationReminderSentDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "ServiceObligationCompleteReminderSentDate",
                table: "StudentInstitutionFundings");

            migrationBuilder.DropColumn(
                name: "CompleteRegistrationReminderSentDate",
                table: "Student");
        }
    }
}
