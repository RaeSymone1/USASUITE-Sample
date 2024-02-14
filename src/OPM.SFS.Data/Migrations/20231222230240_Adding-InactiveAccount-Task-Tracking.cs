using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddingInactiveAccountTaskTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveAccountReminderSentDate",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveAccountReminderSentDate",
                table: "AgencyUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveAccountReminderSentDate",
                table: "AdminUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveAccountReminderSentDate",
                table: "AcademiaUser",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveAccountReminderSentDate",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "InactiveAccountReminderSentDate",
                table: "AgencyUser");

            migrationBuilder.DropColumn(
                name: "InactiveAccountReminderSentDate",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "InactiveAccountReminderSentDate",
                table: "AcademiaUser");
        }
    }
}
